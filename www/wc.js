function loadComic(){
    var pagesURL = "./pages/";
    var page = 1;
        var urlParams;
        (window.onpopstate = function () {
        var match,
                pl     = /\+/g,  // Regex for replacing addition symbol with a space
                search = /([^&=]+)=?([^&]*)/g,
                decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
                query  = window.location.search.substring(1);
        
        urlParams = {};
        while (match = search.exec(query))
        urlParams[decode(match[1])] = decode(match[2]);
        })();
        page = urlParams["page"]*1-1;
        const content = document.getElementById('content');
        const back = document.getElementById('go-back');
        const next = document.getElementById('next-command-link');
        const title = document.getElementById('command');

        next.href="?page="+(page+1);
        back.href="?page="+(page-1);

        //var contentPath = pagesURL + urlParams["page"] + "/" + "content.html";
        //var commandPath = pagesURL + (urlParams["page"]*1+1) + "/" + "command.txt";
        //var titlePath = pagesURL + urlParams["page"] + "/" + "command.txt";

        fetch('./webcomic.json', { 
        method: 'GET'
        })
        .then(function(response) { return response.json(); })
        .then(function(json) {
        content.innerHTML = json[page].content;
        title.innerHTML = json[page].command;
        next.innerHTML = json[page+1].command;
        });
}