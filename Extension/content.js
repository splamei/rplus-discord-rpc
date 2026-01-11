function safeSend(data) {
    if (chrome.runtime && chrome.runtime.id) {
        try {
            chrome.runtime.sendMessage({ type: "RPLUS-RPC-DATA-SENDER", data: data });
        } catch (err) {
            console.warn("Failed to send data! - ", err);
        }
    }
}

function getPageData() {
    let url = window.location.href;

    if (url.startsWith("https://rhythm-plus.com"))
    {
        let songName = document.querySelector('div.detail.py-5 > div:nth-child(1)')?.innerText
        let songAuthor = "";
        let songCharter = "";
        let songTitle = "";
        let songImage = "";

        if (songName)
        {
            songAuthor = document.querySelector('div.detail.py-5 > div:nth-child(2)')?.innerText;
            songCharter = document.querySelector('div.pt-2.text-xs.text-white.text-opacity-25 > span:nth-child(3) > span')?.innerText;
            if (!songCharter)
            {
                songCharter = document.querySelector('div.pt-2.text-xs.text-white.text-opacity-25 > span > span')?.innerText;
            }
            songTitle = document.querySelector('div.detail.py-5 > div:nth-child(1)')?.childNodes[1]?.nodeValue?.trim() || "";
            songImage = document.querySelector('div.detail > div > div > div.image > img').src || "";
        }

        let resultRankObj = document.querySelector('.score')?.innerText;
        let resultAccuracyObj = "";
        let resultScoreObj = "";
        let resultMaxComboObj = "";
        let resultFcObj = "";

        if (resultRankObj)
        {
            resultAccuracyObj = document.querySelector('div:nth-child(3) > span')?.innerText;
            resultScoreObj = document.querySelector('div.rightScore.flex-grow > div:nth-child(1) > span')?.innerText;
            resultMaxComboObj = document.querySelector('div.rightScore.flex-grow > div:nth-child(2) > span')?.innerText;
            resultFcObj = document.querySelector('div.rightScore.flex-grow > div:nth-child(2) > div')?.innerText;
        }

        let currentAccuracyObj = document.querySelector('.score span')?.innerText;
        let currentScoreObj = "";
        let progressElem = "";
        let currentTimeObj = "0%";

        if (currentAccuracyObj)
        {
            currentScoreObj = document.querySelector('.score > span')?.innerText;
            progressElem = document.querySelector('.top-progress');

            if (progressElem) {
                currentTimeObj = progressElem.style.width || getComputedStyle(progressElem).width;
            }
        }


        return {
            title: document.title,
            url: window.location.href,

            selectedSongName: songName || "",
            selectedSongAuthor: songAuthor || "",
            selectedSongCharter: songCharter || "",
            selectedSongTitle: songTitle,
            selectedSongImage: songImage || "",

            resultRank: resultRankObj || "",
            resultAccuracy: resultAccuracyObj,
            resultScore: resultScoreObj || "",
            resultMaxCombo: resultMaxComboObj || "",
            resultFC: resultFcObj || "",

            currentAccuracy: currentAccuracyObj || "",
            currentScore: currentScoreObj || "",
            currentTime: currentTimeObj
        };
    }
    else if (url.startsWith("https://v2.rhythm-plus.com"))
    {
        let songName = document.querySelector('div.flex-1 > div.mt-10 > div')?.innerText
        let songAuthor = "";
        let songCharter = "";
        let songTitle = "";
        let songImage = "";

        if (songName)
        {
            songAuthor = document.querySelector('div.flex-1.self-end > div.mt-10 > div.opacity-60')?.innerText;
            songCharter = document.querySelector('a > div > div > div.text-sm.leading-5 > div > div')?.innerText;
            //if (!songCharter)
            //{
            //    songCharter = document.querySelector('div.pt-2.text-xs.text-white.text-opacity-25 > span > span')?.innerText;
            //}
            songTitle = document.querySelector('div.text-xl > span.w-fit')?.innerText;
            songImage = "";// document.querySelector('div.detail > div > div > div.image > img').src || "";
        }

        let resultRankObj = document.querySelector('.score')?.innerText;
        let resultAccuracyObj = "";
        let resultScoreObj = "";
        let resultMaxComboObj = "";
        let resultFcObj = "";

        if (resultRankObj)
        {
            resultAccuracyObj = document.querySelector('div.percentage-display > div')?.innerText;
            resultScoreObj = document.querySelector('div.score-title > div')?.innerText;
            resultMaxComboObj = document.querySelector('div.combo-container > div > div')?.innerText;
            resultFcObj = document.querySelector('div.combo-container > div.mark-chip.achievement-chip.combo-chip')?.innerText;
        }

        let currentAccuracyObj = document.querySelector('div.score > div:nth-child(2)')?.innerText;
        let currentScoreObj = "";
        let progressElem = "";
        let currentTimeObj = "0%";

        if (currentAccuracyObj)
        {
            currentScoreObj = document.querySelector('div.score > div.text-5xl')?.innerText;
            progressElem = document.querySelector('.top-progress');

            if (progressElem) {
                currentTimeObj = progressElem.style.width || getComputedStyle(progressElem).width;
            }
        }


        return {
            title: document.title,
            url: window.location.href,

            selectedSongName: songName || "",
            selectedSongAuthor: songAuthor || "",
            selectedSongCharter: songCharter || "",
            selectedSongTitle: songTitle,
            selectedSongImage: songImage || "",

            resultRank: resultRankObj || "",
            resultAccuracy: resultAccuracyObj,
            resultScore: resultScoreObj || "",
            resultMaxCombo: resultMaxComboObj || "",
            resultFC: resultFcObj || "",

            currentAccuracy: currentAccuracyObj || "",
            currentScore: currentScoreObj || "",
            currentTime: currentTimeObj
        };
    }
}

function startPolling() {
    console.log("Now polling data for sending to server");
    if (timer) clearInterval(timer);
    timer = setInterval(() => safeSend(getPageData()), updateInterval);
}

document.addEventListener("visibilitychange", () => {
    chrome.storage.sync.get(defaultSettings, (items) => {
        updateInterval = document.hidden ? items.updateInterval * 2 : items.updateInterval;
        startPolling();
    });
});

const defaultSettings = { updateInterval: 2000 };

chrome.storage.local.get(defaultSettings, (items) => {
    setInterval(() => {
        safeSend(getPageData());
    }, items.updateInterval);
});

safeSend(getPageData());
