// load in data from SWAPI
let swapi = "https://swapi.dev/api/people/";
let data = [];
let charPages = [];
let characters = [];
let progress = document.getElementById("loadingBar");
let width = 0;
async function getData() {
  console.log("Fetching data...");
  var startTime = performance.now();
  grid.style.cursor = "wait";
  let result = await fetch(swapi);
  if (result.ok) {
    data = await result.json();
    let itemCount = data.count;
    let pageCount = Math.ceil(itemCount / 10);
    let itemN = 1;
    let pageN = 1;
    while (data.next != null) {
      console.log("Loading Page " + pageN + " of " + pageCount);
      result = await fetch(swapi + "?page=" + pageN);
      if (result.ok) {
        data = await result.json();
        charPages.push(data.results);
        width += 10;
        progress.style.width = width + "%";
      } else {
        console.log("Error reading page " + pageN);
      }
      pageN++;
    }
    grid.style.cursor = "auto";
    console.log("Done!");
    var endTime = performance.now();
    document.getElementById("loadingProgress").style.display = "none";
    console.log(
      `getData() api fetch took ${Math.ceil(
        (endTime - startTime) / 1000
      )} seconds`
    );
    charPages.forEach((array) => {
      array.forEach((index) => {
        characters.push(index);
      });
    });
    sortCharacters();
  } else {
    console.log("Error...");
  }
}

// tests to see if text is api call
function fetchTest(test, clas, el) {
  if (test.startsWith("https://swapi")) {
    return fetchValue(test, clas, el);
  } else {
    return test;
  }
}

// grabs a snippet of data from SWAPI
async function fetchValue(fetchUrl, el) {
  fetch(fetchUrl)
    .then((response) => response.json())
    .then((data) => {
      console.log("Success: fetchValue() returned " + data.name);
      if (
        fetchUrl.startsWith("https://swapi.dev/api/planets/") &&
        !homeworlds.includes(data.name)
      ) {
        homeworlds.push(data.name);
      }
      el.textContent = data.name;
    });
  // .catch((error) => {
  //   console.error("fetchValue() failed a fetch:", error);
  //   return fetchUrl;
  // });
}

// creates a new element, used x3 to create a full entry
function createEntryElement(parent, element, clas, text) {
  let newEl = document.createElement(element);
  newEl.setAttribute("class", clas);
  newEl.setAttribute("data", text);
  let entryname;
  switch (clas) {
    case "entry":
      entryname = newEl.appendChild(document.createElement("h2"));
      break;
    case "tag":
      entryname = newEl.appendChild(document.createElement("p"));
      break;
    default:
      console.log("Error: createEntryElement switch was not tripped");
  }
  entryname.textContent = fetchTest(text, entryname);
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
let grid = document.getElementById("dict-grid");
let headerArray = [];
let homeworlds = [];
function outputData(outputArray) {
  let lastEntry = null;
  headerArray = [];
  outputArray.forEach((entry) => {
    if (sortBy == "charname") {
      if (lastEntry == null || lastEntry.name[0] != entry.name[0]) {
        createHeader(entry.name.charAt(0));
        headerArray.push(entry.name.charAt(0));
      }
    } else if (sortBy == "eyecolor") {
      if (lastEntry == null || lastEntry.eye_color != entry.eye_color) {
        createHeader(entry.eye_color);
        headerArray.push(entry.eye_color);
      }
    }
    let newEntry = createEntryElement(grid, "div", "entry", entry.name);
    createEntryElement(newEntry, "div", "tag", entry.birth_year);
    createEntryElement(newEntry, "div", "tag", entry.eye_color);
    createEntryElement(newEntry, "div", "tag", entry.homeworld);
    console.log("Output: " + entry.name);
    lastEntry = entry;
  });
}

// sorts the data and calls outputData with the modified list
let sortController = document.getElementById("sortController");
sortController.addEventListener("click", sortCharacters);
let sortBy = "charname";
let sortOrder = "asc";
function sortCharacters() {
  reset();
  sortBy = sortSelected.value;
  sortOrder = orderSelected.value;
  characters.sort(sortFunction);
  outputData(characters);
}

// executes the default sort algorithm
function simpleSort(a, b) {
  if (a === b) {
    return 0;
  } else {
    if (sortOrder == "asc") {
      return a < b ? -1 : 1;
    } else {
      return a > b ? -1 : 1;
    }
  }
}

// switch that handles different sort requests
function sortFunction(a, b) {
  switch (sortBy) {
    case "charname":
      return simpleSort(a.name, b.name);
    case "eyecolor":
      return simpleSort(a.eye_color, b.eye_color);
    case "birthyear":
      let a2 = a.birth_year.substr(0, a.birth_year.length - 3);
      let b2 = b.birth_year.substr(0, b.birth_year.length - 3);
      if (a2 == "unkn") return 1;
      if (b2 == "unkn") return -1;
      return simpleSort(parseFloat(a2), parseFloat(b2));
    default:
      console.log("Error: sortFunction switch was not tripped");
  }
}

// removes all elements to allow for a new display to be prepared
function reset() {
  while (grid.firstChild) {
    grid.removeChild(grid.firstChild);
  }
}

// ****** MAIN ******
getData();
