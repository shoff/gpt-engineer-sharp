function getAntiForgeryToken() {
    const elements = document.getElementsByName("__RequestVerificationToken");
    if (elements.length > 0) {
        console.log(`token: ${elements[0].value}`);
        return elements[0].value;
    }
    console.warn("no anti forgery token found!");
    return null;
}