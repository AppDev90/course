function maintainscrollposition() {
    var y = window.scrollY
    console.log(y);
    localStorage.setItem('topposition', y);
}

$(function () {
    var top = localStorage.getItem('topposition');
    window.scroll(0, top);
    localStorage.removeItem('topposition');
});
