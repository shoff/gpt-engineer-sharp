window.scrollToBottomIfNeeded = (elementId) => {
    var element = document.getElementById(elementId);
    // Check if we are already at the bottom
    if (element.scrollHeight - element.scrollTop === element.clientHeight) {
        element.scrollTop = element.scrollHeight;
    }
}