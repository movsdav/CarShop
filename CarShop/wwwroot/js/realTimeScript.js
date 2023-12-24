let realTime = () => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    let channelId = $("#ChatChannelId").val();
    let SenderId = $("#SenderId").val();
    let ReceiverId = $("#ReceiverId").val();
    //channelId = channelId.toString();



    connection.start().then(function () {
        console.log("SignalR connected");

        connection.invoke("JoinChat", channelId);
    }).catch(function (err) {
        console.error(err.toString());
    });

    connection.on("ReceiveMessage", function (message) {
        if (message.senderId !== SenderId) {
            let htmlContent = createMsgBox(message.Content, false);

            $('.chat-body').append(htmlContent);

            console.log("New message received: ", message);
        }
    }
    );

    function sendMessage() {
        let content = $("#msgInput").val();

        if (content !== "") {
            connection.invoke("SendMessage", { "id": 0, "senderId": SenderId, "receiverId": ReceiverId, "chatChannelId": parseInt(channelId), "content": content }).catch(err => console.error(err))

            let htmlContent = createMsgBox(content, true);

            $('.chat-body').append(htmlContent);
        }

        $("#msgInput").val("");
        $("#chatContainer").scrollTop($("#chatContainer")[0].scrollHeight);
    }

    document.getElementById("chatSendMsgBtn").addEventListener("click", sendMessage);
}

let createMsgBox = (content, isSender) => {
    return `<div class="row ${isSender ? 'justify-content-end' : ''}">` +
        '<div class="col-md-6">' +
        `<div class="message-box ${isSender ? 'sender-message' : 'receiver-message'} ">` +
        '<p>' + content + '</p>' +
        '<small class="text-muted">12:30 PM</small>' +
        '</div>' +
        '</div>' +
        '</div>';
};

realTime();

$("#chatContainer").scrollTop($("#chatContainer")[0].scrollHeight);