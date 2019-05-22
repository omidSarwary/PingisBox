// WebSocket = undefined;
//EventSource = undefined;
//, signalR.HttpTransportType.LongPolling
function reRout(controller,action) {
    window.location.href = "/" + controller+"/"+action;
}
// Add active class to the current button (highlight it)


function reRoutId(controller,action,id) {
    window.location.href = "/"+controller+"/"+action+"/"+id;
}


function FormateTime(time) {
    var days = [ "Söndag","Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag", "Lördag"];
    var months = [" januari ", " februari ", " mars ", " april ", " maj ", " juni ", " juli ", " augusti ", " september ", " oktober", " november ", " december "];
    var Time = new Date(time);
    var day = days[Time.getDay()];
    var month = months[Time.getMonth()];
    var date = Time.getDate();
    var hour = Time.getHours();
    var mins = Time.getMinutes();
    var newTime = day + " den " + date + " " + month + " " + hour + ":" + mins;
    return newTime;
}


function removeBedge() {
    document.getElementById("badge").classList.remove("badgee");
    connection.invoke("Reset").catch(function (err) {
        return console.error(err.toString());
    });
}




function notis(msg,title) {
    $.notify({
        // options

        title: title,
        message: msg,


    }, {
            // settings
            element: 'body',
            position: null,
            type: "info",
            allow_dismiss: true,
            newest_on_top: false,

            placement: {
                from: "bottom",
                align: "right"
            },
            offset: 20,
            spacing: 10,
            z_index: 1031,
            delay: 3000,
            timer: 1000,

            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            }


        });

    document.getElementById("badge").classList.add("badgee");
}

let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/BoxHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();


    connection.on("doorStats", (fullName, door, time) => {       
       

        if (door) {

            document.getElementById("doorIcon").className = "fas fa-door-open";
            document.getElementById("BoxStatus").innerHTML = "Open";
            document.getElementById("BoxTime").innerHTML = "Updated at: " +time;
            document.getElementById("BoxUser").innerHTML = "Opened By " + fullName;
           // notis("The Box Door is Opened by: " + fullName,"Box Status");
        }

        if (!door) {
            document.getElementById("doorIcon").className = "fas fa-door-closed";
            document.getElementById("BoxStatus").innerHTML = "Closed";            
            document.getElementById("BoxTime").innerHTML = "Updated at: "+time;
            document.getElementById("BoxUser").innerHTML = "Closed By " + fullName;
            //notis("The Box Door is Closed by: " + fullName, "Box Status");
        }

        
    });

    connection.on("Item1", (fullName, isInBox, time) => {
     

        if (isInBox) {

            document.getElementById("item1Icn").className = "fas fa-table-tennis";
            document.getElementById("item1Stats").innerHTML = "Available";
            document.getElementById("item1Time").innerHTML = "Updated at: " + time;
            document.getElementById("item1User").innerHTML = "Returned By " + fullName;
            //notis("Racket no 1 is returned by: " + fullName,"Item Status");
        }

   
        if (!isInBox) {
            document.getElementById("item1Icn").className = "fas fa-chalkboard";
            document.getElementById("item1Stats").innerHTML = "Not Available";
            document.getElementById("item1Time").innerHTML = "Updated at: " + time;
            document.getElementById("item1User").innerHTML = "Borrowed By " + fullName;
            //notis("Racket no 1 is borrowed by: " + fullName, "Item Status");
        }


    });

    connection.on("Item2", (fullName, isInBox, time) => {
      

        if (isInBox) {

            document.getElementById("item2Icn").className = "fas fa-table-tennis";
            document.getElementById("item2Stats").innerHTML = "Available";
            document.getElementById("item2Time").innerHTML = "Updated at: " + time;
            document.getElementById("item2User").innerHTML = "Returned By " + fullName;
           // notis("Racket no 1 is returned by: " + fullName, "Item Status");
        }

        if (!isInBox) {
            document.getElementById("item2Icn").className = "fas fa-chalkboard";
            document.getElementById("item2Stats").innerHTML = "Not Available";
            document.getElementById("item2Time").innerHTML = "Updated at: " + time;
            document.getElementById("item2User").innerHTML = "Borrowed By " + fullName;
           // notis("Racket no 1 is borrowed by: " + fullName, "Item Status");
        }


    });


    connection.on("TimesBorrowedToday", (times, time,updated) => {

        document.getElementById("day").innerHTML = time;
        document.getElementById("TimesBorrowed").innerHTML = times + " Times";
        document.getElementById("countUpdate").innerHTML = updated;  


    });

    

    connection.on("AddNotification", (message,time) => {

        var li = document.createElement('LI');

        var a = document.createElement("A");
        a.setAttribute("class", "Notia");
        var span = document.createElement("SPAN");
       
        var hr = document.createElement("HR");
        hr.setAttribute("class", "Notihr");
        span.setAttribute("class", "NotiTime");
        var text = document.createTextNode(message);
        var Time = document.createTextNode(time);


        span.appendChild(Time);

        a.appendChild(text);
        a.appendChild(span);
        li.appendChild(a);
       



        var list = document.getElementById("notices");
        list.insertBefore(hr, list.childNodes[0]);
        list.insertBefore(li, list.childNodes[0]);

        notis(message, "New Notification");
    });



    connection.on("AddAllNotifications", (listData,NewChanges) => {
  
        var obj = JSON.parse(listData); 
        var numberOfListItems = obj.length;
        var li = document.createElement('LI');
        var a = document.createElement("A");
        a.setAttribute("onClick", "reRout('Notifications','Index')");
        a.setAttribute("class", "Notia");
        var text = document.createTextNode("Show All Notifications");
        a.appendChild(text);
        li.appendChild(a);
        var list = document.getElementById("notices");
        list.insertBefore(li, list.childNodes[0]);  
        

        for (var i = 0; i < numberOfListItems; i++) {
            var d = document.getElementById('notices');
            var li = document.createElement('LI');
            var a = document.createElement("A");
            a.setAttribute("class", "Notia");
            a.setAttribute("onClick", "reRoutId('Notifications','Details',"+(i+1)+")");
            var span = document.createElement("SPAN");
            var hr = document.createElement("HR");
            hr.setAttribute("class", "Notihr");
            span.setAttribute("class", "NotiTime");
            var text = document.createTextNode(obj[i].Message);
            var time = FormateTime(obj[i].Time);
            var Time = document.createTextNode(time);
            span.appendChild(Time);
            a.appendChild(text);
            a.appendChild(span);
            li.appendChild(a);
            
            d.insertBefore(hr, d.childNodes[0]);
            d.insertBefore(li, d.childNodes[0]);

        }

        if (NewChanges) {
            document.getElementById("badge").classList.add("badgee");
        }
    });  

    connection.start()
        .catch(err => console.error(err.toString()));
    
};

setupConnection();






