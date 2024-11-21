

namespace Application.Common
{
    public enum TasksStatus
    {
        AllTask = 0,
        Requested = 1,
        Pending = 2,
        Inprogress = 3,
        Cancelled = 4,
        Completed = 5,
    }

    public enum WorkerStatus
    {
        Online = 1,
        Offline = 2,
    }
    public enum RequestStatus
    {
        NotViewed,
        Viewed,
        Assigned,
        Working,
        Completed,
        Cancelled
    }
    public enum PaymentMethod
    {
        Stripe,
        Square
    }
    public enum WorkerTaskStatus
    {
        Decline = 0,
        Accept = 1
    }
    public enum ServiceStatus
    {
        Pending,
        Ongoing,
        Paused,
        Completed
    }

    public enum AccountType
    {
        Savings = 1,
        Checking = 2,
        Others = 3
    }
    public enum TransactionType
    {
        Credit = 1,
        Debit = 2
    }
    public enum StripePaymentStatus
    {
        succeeded = 1,
        canceled = 2,
        processing = 3,
        failed = 4
    }
    public enum PaymentStatus
    {
        Completed = 1,
        Pending = 2,
        Declined = 3,
        Canceled = 4,
        Expired = 5,
        Refunded = 6,
        Disputed = 7,
        Offline = 8
    }

    public enum SquarePaymentStatus
    {
        Completed = 1,
        Pending = 2,
        Declined = 3,
        Canceled = 4,
        Expired = 5,
        Refunded = 6,
        Disputed = 7,
        Offline = 8
    }

    public enum Role
    {
        Admin = 1,
        User = 2,
        Worker = 3,
        SuperAdmin = 4,
    }
    public enum AdminUserType
    {
        Admin = 1,
        SuperAdmin = 2
    }


    public enum CustomerNotificationType
    {
        // Task-Related Notifications
        TaskAssigned = 1,
        TaskUpdate = 2,
        PaymentUpdate = 3,
        TaskCreated = 4

    }
    public enum WorkerNotificationType
    {
        TaskAssigned = 1,
        TaskUpdate = 2,
        PaymentUpdate = 3,
        BankDataUpdate =4,
        NewMessage=5
    }

    public enum AdminNotificationType
    {
        // Task-Related Notifications
        TaskUpdate = 1,
        PaymentUpdate = 2,
        NewEmployee = 3,
        NewCustomer = 4,
        CommunicationStarted=5,
        NewTask = 6,
    }





}
