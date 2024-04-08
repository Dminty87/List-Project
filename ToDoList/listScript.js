



function testScript(){
    document.getElementById("output").innerHTML = "Success!"
}


function loadXML(){
    const req = new XMLHttpRequest();
    req.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("listOutput").
        }
    }

    req.open("GET", "GeeSkillet.xml", true)
    req.send()
}

