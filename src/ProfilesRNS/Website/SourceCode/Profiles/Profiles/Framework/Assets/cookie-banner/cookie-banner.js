// Source: https://www.jqueryscript.net/other/cookie-consent-banner-localstroage.html
// License: MIT

/**
 * Shows the Cookie banner
 */
function showCookieBanner() {
  let cookieBanner = document.getElementsByClassName("cookie-banner")[0];
  cookieBanner.style.display = "flex";
}

/**
 * Hides the Cookie banner and saves the value to localstorage
 */
function hideCookieBanner() {
  localStorage.setItem("web_dev_isCookieAccepted", "yes");

  let cookieBanner = document.getElementsByClassName("cookie-banner")[0];
  cookieBanner.style.display = "none";
}

/**
 * Checks the localstorage and shows Cookie banner based on it.
 */
function initializeCookieBanner() {
  let isCookieAccepted = localStorage.getItem("web_dev_isCookieAccepted");

  if (isCookieAccepted === null) {
    localStorage.clear();
    localStorage.setItem("web_dev_isCookieAccepted", "no");

    showCookieBanner();
  }
  if (isCookieAccepted === "no") {
    showCookieBanner();
  }
}

window.onload = initializeCookieBanner();
window.hideCookieBanner = hideCookieBanner;
