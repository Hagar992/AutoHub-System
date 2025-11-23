


//  Search 
const notfoundmsg = document.getElementById("notfound");
const searchInput = document.getElementById('search');
const searchBtn = document.getElementById('searchBtn');
const cards = Array.from(document.querySelectorAll('.card'));

function msg(message = "No Such Cars") {
  notfoundmsg.innerText = message;
  notfoundmsg.style.color = "gray";
  notfoundmsg.style.display= "block";
  notfoundmsg.style.fontSize = "50px";
  notfoundmsg.style.margin = "10px auto";
  notfoundmsg.style.textAlign = "center";
}

function clearmsg() {
  notfoundmsg.innerText = "";
    notfoundmsg.style.display= "none";
}

function search() {
  const value = searchInput.value.trim().toLowerCase();
  let found = false;

  cards.forEach(card => {
    const text = card.innerText.toLowerCase();
    if (text.includes(value)) {
      card.style.display = "block";
      found = true;
    } else {
      card.style.display = "none";
    }
  });

  if (!found) msg();
  else clearmsg();
}

searchBtn.addEventListener('click', search);
searchInput.addEventListener('keypress', (e) => {
  if (e.key === 'Enter') search();
});

searchInput.addEventListener("input", () => {
  const searchText = searchInput.value.trim().toLowerCase();
  if (searchText === "") {
    cards.forEach(car => car.style.display = "block");
    clearmsg();
  }
});



const brandButtons = document.querySelectorAll('#brandsbtns .typbtn');
const statusButtons = document.querySelectorAll('#status .typbtn');
let activeBrands = []; 
let activeStatus = "";


function applyFilters() {
  let found = false;


  // if (startYear !== "default" && endYear !== "default" && parseInt(startYear) > parseInt(endYear)) {
  //   msg(" Invalid Year Range: 'From' must be less than 'To'");
  //   cards.forEach(card => card.style.display = "none");
  //   return;
  // }

  cards.forEach(card => {
    const text = card.innerText.toLowerCase();

  
    const matchBrand =
      (activeBrands.length > 0
        ? activeBrands.some(brand => text.includes(brand.toLowerCase()))
        : true) &&
      (selectedBrand ? text.includes(selectedBrand) : true);

  
    const matchStatus = activeStatus
      ? text.includes(activeStatus.toLowerCase())
      : true;

   
    const yearElements = card.querySelectorAll(".carstatus .cardet");
    let cardYear = null;
    yearElements.forEach(el => {
      const num = parseInt(el.innerText);
      if (!isNaN(num)) cardYear = num;
    });

    const fromYearVal = startYear !== "default" ? parseInt(startYear) : null;
    const toYearVal = endYear !== "default" ? parseInt(endYear) : null;

    let matchYear = true;
    if (fromYearVal && cardYear < fromYearVal) matchYear = false;
    if (toYearVal && cardYear > toYearVal) matchYear = false;


    const priceText = card.querySelector(".money")?.innerText || "";
    const priceMatch = priceText.replace(/[^0-9]/g, "");
    const cardPrice = parseInt(priceMatch);

    let matchPrice = true;
    if (fromPrice && cardPrice < fromPrice) matchPrice = false;
    if (toPrice && cardPrice > toPrice) matchPrice = false;

 
    if (matchBrand && matchStatus && matchYear && matchPrice) {
      card.style.display = "block";
      found = true;
    } else {
      card.style.display = "none";
    }
  });

  if (!found) msg();
  else clearmsg();
}


brandButtons.forEach(button => {
  button.addEventListener('click', () => {
    const brand = button.innerText.trim();


    if (activeBrands.includes(brand)) {
      activeBrands = activeBrands.filter(b => b !== brand);
      button.classList.remove('active');
    } else {
     
      activeBrands.push(brand);
      button.classList.add('active');
    }

    applyFilters();
  });
});


statusButtons.forEach(button => {
  button.addEventListener('click', () => {
    const status = button.innerText.trim().toLowerCase();

    if (activeStatus === status) {
      activeStatus = "";
      button.classList.remove('active');
    } else {
      activeStatus = status;
      statusButtons.forEach(btn => btn.classList.remove('active'));
      button.classList.add('active');
    }

    applyFilters();
  });
});


const startYearSelect = document.getElementById('startyear');
const endYearSelect = document.getElementById('endyear');

let startYear = "default";
let endYear = "default";





startYearSelect.addEventListener('change', (e) => {
  startYear = e.target.value;
  applyFilters();
});

endYearSelect.addEventListener('change', (e) => {
  endYear = e.target.value;
  applyFilters();
});

const fromPriceInput = document.getElementById('fromPrice');
const toPriceInput = document.getElementById('toPrice');

let fromPrice = null;
let toPrice = null;

fromPriceInput.addEventListener('input', () => {
  fromPrice = parseFloat(fromPriceInput.value) || null;
  applyFilters();
});

toPriceInput.addEventListener('input', () => {
  toPrice = parseFloat(toPriceInput.value) || null;
  applyFilters();
});


const orderSelect = document.getElementById('order');
const cardsContainer = document.getElementById('cards');

orderSelect.addEventListener('change', () => {
  const orderBy = orderSelect.value;
  sortCards(orderBy);
});


function sortCards(orderBy) {
  let sortedCards = [...cards]; 

  sortedCards.sort((a, b) => {
    const nameA = a.querySelector('.cartitle').innerText.trim().toLowerCase();
    const nameB = b.querySelector('.cartitle').innerText.trim().toLowerCase();

    const yearA = parseInt(a.querySelector('.carstatus .cardet').innerText) || 0;
    const yearB = parseInt(b.querySelector('.carstatus .cardet').innerText) || 0;

    const priceA = parseInt(a.querySelector('.money').innerText.replace(/[^0-9]/g, '')) || 0;
    const priceB = parseInt(b.querySelector('.money').innerText.replace(/[^0-9]/g, '')) || 0;

    switch (orderBy) {
      case "name":
        return nameA.localeCompare(nameB);
      case "price":
        return priceA - priceB;
      case "year":
        return yearA - yearB;
      default:
        return 0; 
    }
  });


  sortedCards.forEach(card => cardsContainer.appendChild(card));
}


const brandSelect = document.getElementById('brand');
let selectedBrand = "";

if (brandSelect) {
  brandSelect.addEventListener('change', (e) => {
    selectedBrand = e.target.value.trim().toLowerCase();

    if (selectedBrand === "default" || selectedBrand === "") {
      selectedBrand = "";
    }

    applyFilters();
  });
}
