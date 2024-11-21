document.addEventListener("DOMContentLoaded", function () {
    async function connectSignalR() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("wss://api-fieldo.gettaxiusa.com/chatHub", { accessTokenFactory: () => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6Im5hdmVlbmtyMTEzQG1haWxpbmF0b3IuY29tIiwiTmFtZSI6Ik5hdmVlbiBLdW1hciIsIkVtYWlsIjoibmF2ZWVua3IxMTNAbWFpbGluYXRvci5jb20iLCJJZCI6IjExMSIsIlJvbGUiOiJXb3JrZXIiLCJuYmYiOjE3MjI1MTUxNTMsImV4cCI6MTc1NDA1MTE1MywiaWF0IjoxNzIyNTE1MTUzLCJpc3MiOiJtdHItYXV0aC1hcGkiLCJhdWQiOiJtdHItY2xpZW50In0.puaMmGLcYaBQWDuEmRVcD0gjVM-fAbusO1Bl4YrO_Lw" }) // Replace with actual token retrieval method
            .build();

        connection.on("ReceiveMessage", (user, message) => {
            console.log(`Message from ${user}: ${message}`);
        });

        connection.on("MessageRead", (messageId) => {
            console.log(`Message with ID ${messageId} was read.`);
            // Handle message read notification as needed
        });

        try {
            await connection.start();
            console.log("SignalR Connected.");

            // Send a message after connecting
            const sendToUserId = 78; // Replace with the recipient user ID
            const message = "Hello from client!";
            await connection.invoke("SendNewMessage", sendToUserId, message);
            console.log("Message sent after connection.");

            // Simulate marking a message as read after some time
        
                const messageId = 1; // Replace with the actual message ID
                console.log(`Invoking MarkMessageAsRead with messageId: ${messageId}`);
                await connection.invoke("MarkMessageAsRead", messageId);
                console.log(`Marked message with ID ${messageId} as read.`);
         
        } catch (err) {
            console.error(err);
        }
    }

    connectSignalR();
});
