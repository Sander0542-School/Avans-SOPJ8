function toggleDarkMode() {
    const enabled = !isDarkModeEnabled();

    enableDarkMode(enabled);

    window.localStorage.setItem("DarkTheme", enabled);

    toggleButton();
}

function enableDarkMode(enable) {
    if (enable) {
        DarkReader.enable();
    } else {
        DarkReader.disable();
    }
}

function toggleButton() {
    const enabled = isDarkModeEnabled();

    document.getElementById("lightThemeText").style.display = enabled ? "block" : "none";
    document.getElementById("darkThemeText").style.display = !enabled ? "block" : "none";
}

function setTheme() {
    const enabled = isDarkModeEnabled();

    enableDarkMode(enabled);
}

function isDarkModeEnabled() {
    return window.localStorage.getItem("DarkTheme") === 'true';
}

setTheme();

window.onload = function () {
    toggleButton();
}