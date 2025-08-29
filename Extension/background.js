let socket;
let isConnecting = false;
let settings = { serverPort: 55256 };

function connectSocket() {
    if (isConnecting) return;
    isConnecting = true;

    try {
        socket = new WebSocket("ws://localhost:" + settings.serverPort);

        socket.onopen = () => {
            console.log("Connected to server app");
            isConnecting = false;
        };

        socket.onclose = () => {
            console.log("Socket closed. Retrying in 5 secconds");
            isConnecting = false;
            setTimeout(connectSocket, 5000);
        };

        socket.onerror = (err) => {
            console.warn("Socket error! - ", err);
            //socket.close();
        };
    } catch (err) {
        console.error("Failed to init socket! - ", err);
        isConnecting = false;
        setTimeout(connectSocket, 5000);
    }
}

function reconnectSocket() {
    if (socket) socket.close();
    else connectSocket();
}

chrome.storage.local.get(settings, (items) => {
    settings = items;
    connectSocket();
});

chrome.runtime.onInstalled.addListener((details) => {
    if (details.reason === "install" || details.reason === "update") {
        chrome.tabs.create({ url: chrome.runtime.getURL("index.html") });
    }
});

chrome.action.onClicked.addListener((tab) => {
    chrome.tabs.create({ url: chrome.runtime.getURL("index.html") });
});

chrome.runtime.onMessage.addListener((msg) => {
    if (msg.type === "RPLUS-RPC-DATA-SENDER" && socket?.readyState === WebSocket.OPEN) {
        try {
            socket.send(JSON.stringify(msg.data));
        } catch (err) {
            console.warn("Send error! - ", err);
        }
    }
});