var side;
var dark;
var weedth;
var weeedth;
var rate;
var opened = false;
var Closing;
var Opening;

window.onload = function(){
side = document.getElementById("side_bar");
dark = document.getElementById("black");
weedth = window.getComputedStyle(side, null).getPropertyValue("width");
weeedth = parseInt(weedth.replace("px", ""));
rate = 0.05;
}

function CloseSide(){
    clearInterval(Closing);
    clearInterval(Opening);
    var i = parseInt(side_bar.style.left.replace("px", ""))
    Closing = setInterval(
        function(){
            side_bar.style.left = i + "px";
            dark.style.opacity = 1 + (i / weeedth);
            i -= rate * (weeedth + i);
            if (i <= 1 - weeedth) { dark.style.visibility = "hidden"; clearInterval(Closing); }
        }
    , 10)
}

function OpenSide(){
    clearInterval(Closing);
    clearInterval(Opening);
    var i = -1 * weeedth;
    dark.style.visibility = "visible";
    Opening = setInterval(
        function(){
            side_bar.style.left = i + "px";
            dark.style.opacity = 1 + i / weeedth;
            i -= rate * i;
            if (i >= - 1) { opened = true;  clearInterval(Opening); }
        }
    , 10)
}

function Go2Top(){
    window.location.href = "#top";
    CloseSide();
}

function Go2Download(){
    window.location.href = "#download";
    CloseSide();
}

function Go2Steps(){
    window.location.href = "#steps";
    CloseSide();
}

function OnlineVer(){
    window.open("https://covector.github.io/Tetris-Unity/game.html");
}