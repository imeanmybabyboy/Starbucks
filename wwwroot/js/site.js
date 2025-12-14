const animatedChevron = document.getElementById("animated-chevron");
const accountNav = animatedChevron.closest("div");
const dropdownMenu = document.getElementById("account-dropdown");

function toggleAccountMenu() {
    if (dropdownMenu.classList.contains("show")) {
        animatedChevron.classList.add("open");
        animatedChevron.classList.remove("close");
    }
    else {
        animatedChevron.classList.remove("open");
        animatedChevron.classList.add("close");
    }
}

const observer = new MutationObserver(mutations => {
    mutations.forEach(mutation => {
        if (mutation.attributeName === "class") {
            toggleAccountMenu();
        }
    })
})

observer.observe(dropdownMenu, {
    attributes: true,
    attributeFilter: ["class"]
})