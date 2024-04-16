



function testScript(){
    document.getElementById("output").innerHTML = "Success! At current time " + new Date()
}


function loadXML(){
    var req = new XMLHttpRequest();
    req.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {

            var xmlDoc = req.responseXML
            
            //document.documentElement.append(new CDATASection())
            /*
            while(xmlStr.includes("ListItem")){
                xmlStr = xmlStr.replace("ListItem", "li")
            }
            */

            var children = xmlDoc.documentElement.children
            
            var list = new String()

            for (const x of children)
            {
                list += "<li>" + x.textContent + "</li>\n"
            }



            document.getElementById("xmlOut").innerText = "xmlOut: Start:\n" + list + ":end"

            document.getElementById("listOutput").innerHTML = list
            
        }
    }

    req.open("GET", "\\ToDoList\\ToDoList\\bin\\Debug\\ToDoListData\\March 22nd.xml", true)
    req.send()
}

function testReq(){
    var req = new XMLHttpRequest();
    req.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("reqOut").innerHTML = "reqOut: Start:" + req.responseText + ":end"
        }
    }
    req.open("GET", "lorem.txt", true)
    req.send("testing")
}


