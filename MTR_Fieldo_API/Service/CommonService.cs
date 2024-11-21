using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MTR_Fieldo_API.Service
{
    public class CommonService : ICommonService
    {
        private readonly MtrContext _context;
        private readonly ResponseDto _responseDto;
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private static string bucketName;
        private static IAmazonS3 client;
        private readonly ILogService _logService;
        public CommonService(Microsoft.Extensions.Configuration.IConfiguration configuration, MtrContext context, ILogService logService)
        {
            _context = context;
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            var accessKey = _configuration.GetSection("AWS:AccessKey").Value;
            var secretKey = _configuration.GetSection("AWS:SecretKey").Value;
            AmazonS3Config clientConfig = new AmazonS3Config();
            clientConfig.SignatureVersion = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:SignatureVersionDev").Value.ToString() : _configuration.GetSection("AWS:SignatureVersion").Value.ToString());
            clientConfig.RegionEndpoint = RegionEndpoint.USEast1;
            clientConfig.SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA256;
            client = new AmazonS3Client(accessKey, secretKey, clientConfig);
            _responseDto = new ResponseDto();
            _logService = logService;
        }
        public async Task<ResponseDto> UploadFile(UploadClientFileDto uploadClientFileDTO, Fieldo_UserDetails user)
        {
            var _responseDto = new ResponseDto();

            try
            {
                _logService.writeLog("", "file service line 43", user.Id);

                using (var memoryStream = new MemoryStream())
                {
                    // Copy file to memory stream
                    await uploadClientFileDTO.File.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position to the beginning

                    // Create unique file name to prevent collisions
                    var fileName = user.Id.ToString() + "_" + Guid.NewGuid() + "_" + uploadClientFileDTO.FileName;

                    var request = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = fileName,
                        InputStream = memoryStream, // Use the existing memory stream
                        ContentType = uploadClientFileDTO.File.ContentType,
                        CannedACL = S3CannedACL.Private // Ensure this is correct for your use case
                    };

                    _logService.writeLog("", "file service line 63", user.Id);

                    var response = await client.PutObjectAsync(request);
                    _logService.writeLog("", "file service line 66", user.Id);

                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        _logService.writeLog("", "file service line 70", user.Id);

                        var url = GeneratePreSignedURL(bucketName, fileName, 6000);

                        _responseDto.Result = new Dictionary<object, object>
                {
                    { "key", fileName },
                    { "fileUrl", url }
                };
                        _responseDto.IsSuccess = true;
                        _responseDto.Message = "File uploaded successfully.";
                    }
                    else
                    {
                        _logService.writeLog("", $"file service line 84: Upload failed with status code {response.HttpStatusCode}", user.Id);

                        _responseDto.Message = "Some server error occurred. Please try again!!";
                        _responseDto.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.writeLog(ex.ToString(), "file service line 91", user.Id);

                _responseDto.IsSuccess = false;
                _responseDto.Message = $"An error occurred while uploading the file: {ex.Message}\n{ex.InnerException?.Message}";
                // Log the exception for debugging purposes
            }

            return _responseDto;
        }

        public async Task<ResponseDto> UploadFile1(UploadClientFileDto uploadClientFileDTO)
        {
            var _responseDto = new ResponseDto();

            try
            {

                using (var memoryStream = new MemoryStream())
                {
                    // Copy file to memory stream
                    await uploadClientFileDTO.File.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position to the beginning

                    // Create unique file name to prevent collisions
                    var fileName =  Guid.NewGuid() + "_" + uploadClientFileDTO.FileName;

                    var request = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = fileName,
                        InputStream = memoryStream, // Use the existing memory stream
                        ContentType = uploadClientFileDTO.File.ContentType,
                        CannedACL = S3CannedACL.Private // Ensure this is correct for your use case
                    };


                    var response = await client.PutObjectAsync(request);

                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {

                        var url = GeneratePreSignedURL(bucketName, fileName, 6000);

                        _responseDto.Result = new Dictionary<object, object>
                {
                    { "key", fileName },
                    { "fileUrl", url }
                };
                        _responseDto.IsSuccess = true;
                        _responseDto.Message = "File uploaded successfully.";
                    }
                    else
                    {
                      
                        _responseDto.Message = "Some server error occurred. Please try again!!";
                        _responseDto.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = $"An error occurred while uploading the file: {ex.Message}\n{ex.InnerException?.Message}";
                // Log the exception for debugging purposes
            }

            return _responseDto;
        }

        //public async Task<ResponseDto> UploadFile(UploadClientFileDto uploadClientFileDTO, Fieldo_UserDetails user)
        //{

        //    try
        //    {
        //        byte[] fileBytes = new Byte[uploadClientFileDTO.File.Length];
        //        uploadClientFileDTO.File.OpenReadStream().Read(fileBytes, 0, Int32.Parse(uploadClientFileDTO.File.Length.ToString()));

        //        // create unique file name for prevent the mess
        //        var fileName = user.Id.ToString() + "_" + Guid.NewGuid() + "_" + uploadClientFileDTO.FileName;

        //        PutObjectResponse response = null;

        //        using (var stream = new MemoryStream(fileBytes))
        //        {
        //            var request = new PutObjectRequest
        //            {
        //                BucketName = bucketName,
        //                Key = fileName,
        //                InputStream = stream,
        //                ContentType = uploadClientFileDTO.File.ContentType,
        //                CannedACL = S3CannedACL.Private
        //            };

        //            response = await client.PutObjectAsync(request);
        //        };

        //        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            var url = GeneratePreSignedURL(bucketName, fileName, 6000);

        //            _responseDto.Result = new Dictionary<object, object>
        //            {
        //                { "key", fileName },
        //                { "fileUrl", url }
        //            };
        //            _responseDto.IsSuccess = true;


        //        }
        //        else
        //        {
        //            _responseDto.Message = "Some server error occured. Please try again!!";
        //            _responseDto.IsSuccess = false;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _responseDto.IsSuccess = false;
        //        _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
        //    }
        //    return _responseDto; 
        //}

        public async Task<(string key, bool isSuccess)> UploadFile(UploadClientFileDto uploadClientFileDTO)
        {

            try
            {
                byte[] fileBytes = new Byte[uploadClientFileDTO.File.Length];
                uploadClientFileDTO.File.OpenReadStream().Read(fileBytes, 0, Int32.Parse(uploadClientFileDTO.File.Length.ToString()));

                // create unique file name for prevent the mess
                var fileName =Guid.NewGuid() + "_" + uploadClientFileDTO.FileName;

                PutObjectResponse response = null;

                using (var stream = new MemoryStream(fileBytes))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = fileName,
                        InputStream = stream,
                        ContentType = uploadClientFileDTO.File.ContentType,
                        CannedACL = S3CannedACL.Private
                    };

                    response = await client.PutObjectAsync(request);
                };

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var url = GeneratePreSignedURL(bucketName, fileName, 6000);

                    return (fileName, true);


                }
                else
                {
                    return ("", false);

                }

            }
            catch (Exception ex)
            {
                return ("", false);
            }
   
        }

        public string GeneratePreSignedURL(string awsBucketName, string key, int expireInSeconds)

        {

            string urlString = string.Empty;

            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest

            {

                BucketName = awsBucketName,

                Key = key,

                Expires = DateTime.Now.AddSeconds(expireInSeconds)

            };

            urlString = client.GetPreSignedURL(request);
            return urlString;

        }

        public (bool Succeeded, string Error) SendEmail(string TemplateCode, EmailInformationDto emailInformation, string receiverEmail = "")
        {

            bool flag = false;
            string error = "";
            try
            {
                if (TemplateCode != null)
                {
                    var launguageID = _context.Fieldo_UserDetails.Where(x => x.Email == emailInformation.EmailAddress).Select(x => x.LanguageId).FirstOrDefault();
                    if (launguageID == null)
                    {
                        launguageID = "en-US";
                    }
                    var result = _context.Fieldo_EmailTemplate.Where(s => s.TemplateName == TemplateCode && s.LanguageId == launguageID).FirstOrDefault();
                    if (result == null)
                    {
                        result = _context.Fieldo_EmailTemplate.Where(s => s.TemplateName == TemplateCode && s.LanguageId == "en-US").FirstOrDefault();
                    }

                    if (result.Id != null)
                    {
                            error = SendEmail(result, emailInformation, receiverEmail);
                            if (error == "Success") { flag = true; }
                    }

                }
                return (flag, error);
            }
            catch (Exception Ex)
            {
                return (flag, Ex.Message);
            }
        }

        private string SendEmail(Fieldo_EmailTemplate result, EmailInformationDto emailInformation, string receiverEmail = "")
        {
            string error = "";
            string body = _context.Fieldo_GenericSetting.Where(s => s.SettingName == "EMAILTEMPLATE").Select(s => s.DefaultTextMax).FirstOrDefault();
            var usr = _context.Fieldo_UserDetails.Where(x => x.Email.ToLower() == emailInformation.EmailAddress.ToLower()).FirstOrDefault();

            if (usr != null)
            {
                if (usr.FirstName.Contains(" "))
                {
                    var commands = usr.FirstName.Split(' ', 2);
                    usr.FirstName = commands[0];
                }
            }

            try
            {

                if (body.IndexOf("#content#") > 0)
                {
                    body = body.Replace("#content#", result.Body);
                }
                if (body.IndexOf("#CURRENTDATE#") > 0)
                {
                    body = body.Replace("#CURRENTDATE#", DateTime.Now.Year.ToString());
                }
                if (body.IndexOf("#CUSTOMERNAME#") > 0)
                {
                    body = body.Replace("#CUSTOMERNAME#", emailInformation.UserName);
                }
             
                if (body.IndexOf("#User#") > 0)
                {
                    body = body.Replace("#User#", (usr != null ? usr.FirstName : emailInformation.UserName));
                }
                if (body.IndexOf("#LogoLink#") > 0)
                {
                    body = body.Replace("#LogoLink#", _configuration.GetSection("exchangeUrl:LogoLink").Value);
                }
                if (body.IndexOf("#IPADDRESS#") > 0)
                {
                    body = body.Replace("#IPADDRESS#", emailInformation.IpAddress);
                }
                if (body.IndexOf("#MAXWITHDRAWALLIMIT#") > 0)
                {
                    body = body.Replace("#MAXWITHDRAWALLIMIT#", emailInformation.MaximumWithdrawalLimit.ToString());
                }
                if (body.IndexOf("#MINWITHDRAWALLIMIT#") > 0)
                {
                    body = body.Replace("#MINWITHDRAWALLIMIT#", emailInformation.MinimumWithdrawalLimit.ToString());
                }
              
                if (body.IndexOf("#REASON#") > 0)
                {
                    body = body.Replace("#REASON#", emailInformation.Reason);
                }
                if (body.IndexOf("#Country#") > 0)
                {
                    body = body.Replace("#Country#", emailInformation.Location);
                }
                if (body.IndexOf("#Browser#") > 0)
                {
                    body = body.Replace("#Browser#", emailInformation.Browser);
                }
                if (body.IndexOf("#Agent#") > 0)
                {
                    body = body.Replace("#Agent#", emailInformation.Browser);
                }
    
                if (body.IndexOf("#ContactLink#") > 0)
                {
                    body = body.Replace("#ContactLink#", _configuration.GetSection("exchangeUrl:ContactLink").Value);
                }
              
                if (body.IndexOf("#ResetPassLink#") > 0)
                {
                    body = body.Replace("#ResetPassLink#", emailInformation.ResetPassLink);
                }
                if (body.IndexOf("#ActivationLink#") > 0)
                {
                    body = body.Replace("#ActivationLink#", emailInformation.ActivationLink);
                }
            
                if (body.IndexOf("#Email#") > 0)
                {
                    body = body.Replace("#Email#", emailInformation.EmailAddress);
                }
                if (body.IndexOf("#TermLink#") > 0)
                {
                    body = body.Replace("#TermLink#", _configuration.GetSection("WebUrl:TermLink").Value);
                }
                if (body.IndexOf("#TelegramLink#") > 0)
                {
                    body = body.Replace("#TelegramLink#", _configuration.GetSection("WebUrl:TelegramLink").Value);
                }
                if (body.IndexOf("#FacebookLink#") > 0)
                {
                    body = body.Replace("#FacebookLink#", _configuration.GetSection("WebUrl:FacebookLink").Value);
                }
                if (body.IndexOf("#TwitterLink#") > 0)
                {
                    body = body.Replace("#TwitterLink#", _configuration.GetSection("WebUrl:TwitterLink").Value);
                }
                if (body.IndexOf("#MediumLink#") > 0)
                {
                    body = body.Replace("#MediumLink#", _configuration.GetSection("WebUrl:MediumLink").Value);
                }
                if (body.IndexOf("#RedditLink#") > 0)
                {
                    body = body.Replace("#RedditLink#", _configuration.GetSection("WebUrl:RedditLink").Value);
                }
                if (body.IndexOf("#YoutubeLink#") > 0)
                {
                    body = body.Replace("#YoutubeLink#", _configuration.GetSection("WebUrl:YoutubeLink").Value);
                }
                if (body.IndexOf("#SlackLink#") > 0)
                {
                    body = body.Replace("#SlackLink#", _configuration.GetSection("WebUrl:SlackLink").Value);
                }
                if (body.IndexOf("#SteemitLink#") > 0)
                {
                    body = body.Replace("#SteemitLink#", _configuration.GetSection("WebUrl:SteemitLink").Value);
                }
                if (body.IndexOf("#LinkedinLink#") > 0)
                {
                    body = body.Replace("#LinkedinLink#", _configuration.GetSection("WebUrl:LinkedinLink").Value);
                }
                if (body.IndexOf("#InstagramLink#") > 0)
                {
                    body = body.Replace("#InstagramLink#", _configuration.GetSection("WebUrl:InstagramLink").Value);
                }
                if (body.IndexOf("#WebLink#") > 0)
                {
                    body = body.Replace("#WebLink#", _configuration.GetSection("WebUrl:MailToLink").Value);
                }
                if (body.IndexOf("#OrderId#") > 0)
                {
                    body = body.Replace("#OrderId#", emailInformation.OrderId);
                }
                if (body.IndexOf("#Message#") > 0)
                {
                    body = body.Replace("#Message#", emailInformation.Message);
                }
                if (body.IndexOf("#FromEmail#") > 0)
                {
                    body = body.Replace("#FromEmail#", emailInformation.FromEmail);
                }
                if (body.IndexOf("#price#") > 0)
                {
                    body = body.Replace("#price#", emailInformation.Price);
                }
               
                if (body.IndexOf("#creditamount#") > 0)
                {
                    body = body.Replace("#creditamount#", emailInformation.CreditAmount.ToString());
                }
                if (body.IndexOf("#debitamount#") > 0)
                {
                    body = body.Replace("#debitamount#", emailInformation.DebitAmount.ToString());
                }
              
                if (body.IndexOf("#REQUESTNUMBER#") > 0)
                {
                    body = body.Replace("#REQUESTNUMBER#", emailInformation.RequestNumber);
                }

                if (body.IndexOf("#AMOUNT#") > 0)
                {
                    body = body.Replace("#AMOUNT#", emailInformation.Amount);
                }

                if (body.IndexOf("#USERID#") > 0)
                {
                    body = body.Replace("#USERID#", emailInformation.UserId);
                }


                if (body.IndexOf("#TRANSACTIONID#") > 0)
                {
                    body = body.Replace("#TRANSACTIONID#", emailInformation.TransactionId);
                }
              

                if (body.IndexOf("#DATETIME#") > 0)
                {
                    body = body.Replace("#DATETIME#", DateTime.UtcNow.ToString());
                }

                if (body.IndexOf("#UNSUBSCRIBELIINK#") > 0)
                {
                    body = body.Replace("#UNSUBSCRIBELIINK#", emailInformation.UnsubscribeLink);
                }

                if (body.IndexOf("#NEWSLETTERTITLE#") > 0)
                {
                    body = body.Replace("#NEWSLETTERTITLE#", emailInformation.NewsLetterTitle);
                }

                if (body.IndexOf("#NEWSLETTERSUMMARY#") > 0)
                {
                    body = body.Replace("#NEWSLETTERSUMMARY#", emailInformation.NewsLetterSummery);
                }

                if (body.IndexOf("#NEWSLETTERLIINK#") > 0)
                {
                    body = body.Replace("#NEWSLETTERLIINK#", emailInformation.NewsLetterLink);
                }

                if (body.IndexOf("#Password#") > 0)
                {
                    body = body.Replace("#Password#", emailInformation.Password);
                }

                SmtpClient smtp = new SmtpClient
                {
                    Host = result.HostName,
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(result.SMTPUsername, result.SMTPPassword),

                };
                MailMessage message = new MailMessage(result.SenderEmail, string.IsNullOrEmpty(receiverEmail) ? emailInformation.EmailAddress : receiverEmail, result.Subject, body);
                message.From = new MailAddress(result.SenderEmail, "FieldOps");
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                message.IsBodyHtml = true;
                try
                {
                    smtp.Send(message);
                }
                catch (SmtpFailedRecipientException ex)
                {
                    // ex.FailedRecipient and ex.GetBaseException() should give you enough info.
                }
                error = "Success";

                string status = message.DeliveryNotificationOptions.ToString();
                if (status == "OnSuccess")
                { emailInformation.IsActive = true; }
                if (usr != null)
                {
                  ////  InsertSentEmail(result, emailInformation, usr, body);
                }
            }
            catch (Exception Ex)
            {
                error = Ex.ToString();
            }
            return error;
        }


    }
}

