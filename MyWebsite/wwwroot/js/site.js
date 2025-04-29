function sortComments(){
    var sortOrder = document.getElementById("sortOrder").value;
    window.location.href = window.location.pathname + "?sortOrder=" + sortOrder;
}