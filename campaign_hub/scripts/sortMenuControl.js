let menuToggle = document.getElementById("openSorter");
let menuElement = document.getElementById("sorterMenu");
function openSorter() {
  if (menuElement.style.display == "flex") {
    menuElement.style.display = "none";
  } else {
    menuElement.style.display = "flex";
  }
}
menuToggle.addEventListener("click", openSorter);
let orderSelected = document.getElementById("order");
let sortSelected = document.getElementById("sortBy");
let valueMenu = document.getElementById("selectValues");
function expandedMenu() {
  if (orderSelected.value == "values") {
    valueMenu.style.display = "flex";
    expandedSwitch();
  } else {
    valueMenu.style.display = "none";
    expandedReset();
  }
}
orderSelected.addEventListener("change", expandedMenu);
orderSelected.addEventListener("change", function () {
  sortOrder = orderSelected.value;
  expandedSwitch();
});
sortSelected.addEventListener("change", function () {
  sortBy = sortSelected.value;
  expandedSwitch();
});
function expandedReset() {
  while (valueMenu.firstChild) valueMenu.removeChild(valueMenu.firstChild);
}
function expandedSwitch() {
  expandedReset();
  switch (sortBy) {
    case "charname":
      characters.forEach((c) => {
        let newP = document.createElement("p");
        let newcheckbox = document.createElement("input");
        let newLabel = document.createElement("label");
        newcheckbox.type = "checkbox";
        newcheckbox.name = "charnameFilter";
        newcheckbox.value = c.name;
        newcheckbox.id = "c" + c.name;
        newLabel.for = "c" + c.name;
        newLabel.textContent = c.name;
        newP.appendChild(newcheckbox);
        newP.appendChild(newLabel);
        valueMenu.appendChild(newP);
      });
      break;
    case "eyecolor":
      let usedColors = [];
      characters.forEach((c) => {
        if (!usedColors.includes(c.eye_color)) {
          usedColors.push(c.eye_color);
          let newP = document.createElement("p");
          let newcheckbox = document.createElement("input");
          let newLabel = document.createElement("label");
          newcheckbox.type = "checkbox";
          newcheckbox.name = "eyecolorFilter";
          newcheckbox.value = c.eye_color;
          newcheckbox.id = "c" + c.eye_color;
          newLabel.for = "c" + c.eye_color;
          newLabel.textContent = c.eye_color;
          newP.appendChild(newcheckbox);
          newP.appendChild(newLabel);
          valueMenu.appendChild(newP);
        }
      });
      break;
    case "birthyear":
      let eras = ["originals", "prequels"];
      eras.forEach((c) => {
        let newP = document.createElement("p");
        let newcheckbox = document.createElement("input");
        let newLabel = document.createElement("label");
        newcheckbox.type = "checkbox";
        newcheckbox.name = "birthyearFilter";
        newcheckbox.value = c;
        newcheckbox.id = "c" + c;
        newLabel.for = "c" + c;
        newLabel.textContent = c;
        newP.appendChild(newcheckbox);
        newP.appendChild(newLabel);
        valueMenu.appendChild(newP);
      });
    case "homeworld":
      homeworlds.forEach((c) => {
        let newP = document.createElement("p");
        let newcheckbox = document.createElement("input");
        let newLabel = document.createElement("label");
        newcheckbox.type = "checkbox";
        newcheckbox.name = "birthyearFilter";
        newcheckbox.value = c;
        newcheckbox.id = "c" + c;
        newLabel.for = "c" + c;
        newLabel.textContent = c;
        newP.appendChild(newcheckbox);
        newP.appendChild(newLabel);
        valueMenu.appendChild(newP);
      });
    default:
      console.log("");
  }
}
