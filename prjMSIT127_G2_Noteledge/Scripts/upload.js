var feedback = function(res) {
    if (res.success === true) {
        var get_link = res.data.link.replace(/^http:\/\//i, 'https://');
        //document.querySelector('.status').classList.add('bg2');
        document.querySelector('.status').innerHTML =
            '<br><input class="image-url" value=\"' + get_link + '\"/>';

        var Allimg = document.querySelectorAll('.myimg');
        var Allimgurl = document.querySelectorAll('.imageurltxt');
        var myurl = document.getElementsByClassName('image-url')[0].defaultValue;

        for (i = 0; i < Allimgurl.length; i++) {

            if (Allimgurl[i].value == "") {
                Allimgurl[i].value = myurl;
                Allimg[i].src = myurl;
                return;
            }

        }
    }
};

new Imgur({
    clientid: '63b01c7dd87dd9d', //You can change this ClientID
    callback: feedback
});