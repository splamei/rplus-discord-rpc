const defaultSettings = {
    serverPort: 55256,
    updateInterval: 2000
};

document.addEventListener("DOMContentLoaded", () => {
    chrome.storage.local.get(defaultSettings, (items) => {
        document.getElementById("serverPort").value = items.serverPort;
        document.getElementById("updateInterval").value = items.updateInterval;
    });
});

document.getElementById("settingsForm").addEventListener("submit", (e) => {
    e.preventDefault();
    const port = parseInt(document.getElementById("serverPort").value, 10);
    const interval = parseInt(document.getElementById("updateInterval").value, 10);

    chrome.storage.local.set({ serverPort: port, updateInterval: interval }, () => {
        alert("Settings saved. They will take full effect when you restart your browser");
    });
});
