class Base64 {
    static #textEncoder = new TextEncoder();
    static #textDecoder = new TextDecoder();

    // https://datatracker.ietf.org/doc/html/rfc4648#section-4
    static encode = (str) => btoa(String.fromCharCode(...Base64.#textEncoder.encode(str)));
    static decode = (str) => Base64.#textDecoder.decode(Uint8Array.from(atob(str), c => c.charCodeAt(0)));

    // https://datatracker.ietf.org/doc/html/rfc4648#section-5
    static encodeUrl = (str) => this.encode(str).replace(/\+/g, '-').replace(/\//g, '_');
    static decodeUrl = (str) => this.decode(str.replace(/\-/g, '+').replace(/\_/g, '/'));

    static jwtEncodeBody = (header, payload) => this.encodeUrl(JSON.stringify(header)) + '.' + this.encodeUrl(JSON.stringify(payload));
    static jwtDecodePayload = (jwt) => JSON.parse(this.decodeUrl(jwt.split('.')[1]));
}

// login validation
let loginInput = document.getElementById("floatingLogin");
let loginLabel = document.getElementById("floatingLoginLabel");
let loginFirstFocus = true;

loginInput.addEventListener("focusin", (e) => {
    loginInput.focused = true;

    if (loginInput.value.length === 0) {
        if (loginFirstFocus) {
            loginLabel.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            loginInput.style.border = "2px solid #00754A";
            loginFirstFocus = false;
        } else {
            loginLabel.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            loginInput.style.border = "2px solid #C82014";
            loginLabel.style.color = "#C82014";
        }
    }
    else {
        loginInput.style.border = "2px solid #00754A";
        loginLabel.style.color = "#00754A";
    }
});

loginInput.addEventListener("focusout", () => {
    if (loginInput.value.trim().length === 0) {
        loginLabel.style.transform = "scale(1) translateY(0) translateX(0)";
        loginInput.style.border = "1px solid #C82014";
        loginInput.classList.add("is-invalid");
        loginLabel.style.color = "#C82014";
    }
    else {
        loginInput.style.border = "1px solid #00754A";
    }
})

loginInput.addEventListener('input', (e) => {
    let login = e.target.value.trim();
    let loginValidationCont = document.getElementById("loginValidation");

    if (login.length === 0) {
        if (loginInput.focused) {
            loginInput.style.border = "2px solid #C82014";
        }
        loginValidationCont.style.display = "flex";
        loginInput.classList.add("is-invalid");
        loginInput.classList.remove("is-valid");
        loginLabel.style.color = "#C82014";
    }
    else {
        if (loginInput.focused) {
            loginInput.style.border = "2px solid #00754A";
        }
        loginValidationCont.style.display = "none";
        loginInput.classList.remove("is-invalid");
        loginInput.classList.add("is-valid");
        loginLabel.style.color = "#00754A";
    }
})


// password validation
let passwordInput = document.getElementById("floatingPassword")
let passwordLabel = document.getElementById("floatingPasswordLabel")
let passwordFirstFocus = true;

passwordInput.addEventListener("focusin", (e) => {
    passwordInput.focused = true;

    if (passwordInput.value.length === 0) {
        if (passwordFirstFocus) {
            passwordLabel.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            passwordInput.style.border = "2px solid #00754A";
            passwordFirstFocus = false;
        } else {
            passwordLabel.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            passwordInput.style.border = "2px solid #C82014";
            passwordLabel.style.color = "#C82014";
        }
    }
    else {
        passwordInput.style.border = "2px solid #00754A";
        passwordLabel.style.color = "#00754A";
    }
});

passwordInput.addEventListener("focusout", () => {
    if (passwordInput.value.trim().length === 0) {
        passwordLabel.style.transform = "scale(1) translateY(0) translateX(0)";
        passwordInput.style.border = "1px solid #C82014";
        passwordInput.classList.add("is-invalid");
        passwordLabel.style.color = "#C82014";
    }
    else {
        passwordInput.style.border = "1px solid #00754A";
    }
})

passwordInput.addEventListener('input', (e) => {
    let password = e.target.value;
    let passValidationCont = document.getElementById("passValidation");

    if (password.length === 0) {
        if (passwordInput.focused) {
            passwordInput.style.border = "2px solid #C82014";
        }
        passValidationCont.style.display = "flex";
        passwordInput.classList.add("is-invalid");
        passwordInput.classList.remove("is-valid");
        passwordLabel.style.color = "#C82014";
    }
    else {
        if (passwordInput.focused) {
            passwordInput.style.border = "2px solid #00754A";
        }
        passValidationCont.style.display = "none";
        passwordInput.classList.remove("is-invalid");
        passwordInput.classList.add("is-valid");
        passwordLabel.style.color = "#00754A";
    }
})


// submiting form

document.addEventListener("submit", (e) => {
    const form = e.target;

    if (form && form["id"] == "auth-form") {
        e.preventDefault();
        const formData = new FormData(form);
        const login = formData.get("user-name-email");
        const password = formData.get("user-password");

        // login and password sumbit validation
        let loginInvalid = login.trim().length === 0;
        let passwordInvalid = password.trim().length === 0;

        if (loginInvalid) {
            loginInput.classList.add("is-invalid");
            loginInput.classList.remove("is-valid");
            loginLabel.style.color = "#C82014";
            loginInput.focus();
            loginInput.style.border = "2px solid #C82014";
        }
        else if (passwordInvalid) {
            passwordInput.classList.add("is-invalid");
            passwordInput.classList.remove("is-valid");
            passwordLabel.style.color = "#C82014";
            passwordInput.focus();
            passwordInput.style.border = "2px solid #C82014";
        }
        else {
            const userPass = login + ":" + password;
            const basicCredentials = Base64.encode(userPass);
            const header = "Authorization: Basic " + basicCredentials;

            fetch("/Account/Authenticate", {
                method: "GET",
                headers: {
                    Authorization: "Basic " + basicCredentials
                }
            }).then(r => r.json())
                .then(j => {
                    if (j.status === "Ok") {
                        window.location.href = j.redirect
                    }
                    else {
                        console.log(j)
                    }
                })
        }
    }
})

const submitBtn = document.getElementById("submit-btn");
submitBtn.addEventListener("mousedown", (e) => {
    e.target.classList.remove("shadow");
    e.target.classList.add("shadow-sm");
    e.target.style.transform = "translateY(5px)"
})

submitBtn.addEventListener("mouseup", (e) => {
    e.target.classList.remove("shadow-sm");
    e.target.classList.add("shadow");
    e.target.style.transform = "translateY(0)"
})

submitBtn.addEventListener("mouseleave", (e) => {
    e.target.classList.remove("shadow-sm");
    e.target.classList.add("shadow");
    e.target.style.transform = "translateY(0)"
})

const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]')
const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl))

const popoverDetails = new bootstrap.Popover('#details', {
    container: 'body'
})

popoverDetails.setContent({
    ".popover-body": "Checking this box will reduce the number of times you’re asked to sign in. To keep your account secure, use this option only on your personal devices.",
})

const popoverForgotUsername = new bootstrap.Popover('#forgot-username', {
    container: 'body'
})

popoverForgotUsername.setContent({
    ".popover-body": "You can now use your email instead of a username.",
})