let api_guilds = "encyclopedia.api/guilds.json";
let api_characters = "encyclopedia.api/character.json";
let api = [api_guilds, api_characters];
let data = { root: [], characters: [], guilds: [] };
let grid = document.getElementById("dict-grid");

async function getData() {
    console.log("Fetching data...");
    var startTime = performance.now();
    grid.style.cursor = "wait";
    api.forEach(async (source) => {
        let result = await fetch(source);
        if (result.ok) {
            data = await result.json();
            outputData(data.results);
        } else {
            console.log("Error...");
        }
    })
    grid.style.cursor = "auto";
    console.log("Done!");
    var endTime = performance.now();
    document.getElementById("loadingProgress").style.display = "none";
    console.log(
        `getData() api fetch took ${Math.ceil(
            (endTime - startTime) / 1000
        )} seconds`
    );
    // charPages.forEach((array) => {
    //     array.forEach((index) => {
    //         characters.push(index);
    //     });
    // });
    // sortCharacters();
}

// creates a new element, used x3 to create a full entry
function createEntryElement(parent, element, clas, text) {
    let newEl = document.createElement(element);
    newEl.setAttribute("class", clas);
    newEl.setAttribute("data", text);
    let entryname;
    switch (clas) {
        case "entry":
            let newImg = document.createElement("img")
            newImg.setAttribute("src", text);
            newImg.setAttribute("width","150");
            newImg.setAttribute("height","150");
            entryname = newEl.appendChild(newImg);
            break;
        case "tag":
            entryname = newEl.appendChild(document.createElement("p"));
            break;
        case "panel":
            entryname = newEl.appendChild(document.createElement("p"));
            break;
        case "entryData":
            entryname = newEl.appendChild(document.createElement("h2"));
            break;
        default:
            console.log("Error: createEntryElement switch was not tripped");
    }
    //entryname.textContent = fetchTest(text, entryname);
    entryname.textContent = text;
    parent.appendChild(newEl);
    return newEl;
}

// creates a header that will separate groups of entries
function createHeader(insertTitle) {
    let headElement = document.createElement("h1");
    headElement.textContent = "~ " + insertTitle + " ~";
    headElement.setAttribute("id", insertTitle);
    grid.appendChild(headElement);
}

// repeatedly calls createEntryElement for each entry
let headerArray = [];
//let homeworlds = [];
function outputData(outputArray) {
    let lastEntry = null;
    headerArray = [];
    outputArray.forEach((entry) => {
        if (sortBy == "name") {
            if (lastEntry == null || lastEntry.name[0] != entry.name[0]) {
                createHeader(entry.name.charAt(0));
                headerArray.push(entry.name.charAt(0));
            }
        } /*else if (sortBy == "eyecolor") {
            if (lastEntry == null || lastEntry.eye_color != entry.eye_color) {
                createHeader(entry.eye_color);
                headerArray.push(entry.eye_color);
            }
        }*/
        let newEntry = createEntryElement(grid, "div", "entry", entry.image);
        let entryData = createEntryElement(newEntry, "div", "entryData", entry.name);
        entry.tags.forEach((tag) => { createEntryElement(entryData, "div", "tag", tag); });
        createEntryElement(newEntry, "div", "panel", entry.description);
        loadPanel(newEntry);
        console.log("Output: " + entry.name);
        lastEntry = entry;
    });
}

var active = null;
function loadPanel(entry) {
    entry.addEventListener("click", function () {
        var panel_p = this.lastElementChild.lastElementChild;
        // add or remove active status
        if (active == this) {
            panel_p.style.display = "none";
            active.classList.toggle("active");
            active = null;
        } else {
            // auto-remove last selection (if any)
            if (active != null) {
                active.classList.toggle("active");
                active.lastElementChild.lastElementChild.style.display = "none";
            }
            active = this;
            active.classList.toggle("active");
            panel_p.style.display = "block";
        }
    })
}

getData();