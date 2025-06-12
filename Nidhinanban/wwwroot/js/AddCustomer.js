    var profile=document.getElementById('profilePic');
    var house=document.getElementById('housePic');
    var pan=document.getElementById('panPic');
    var aadhaar=document.getElementById('aadhaarPic');
    let camera=null;
    function snap(Cameraholder, camera, container) {
    document.getElementById(Cameraholder).style = "display: block;";
    document.getElementById("profileuploadbtn").value="";
    currentCameraApp = setupCameraApp({
        videoContainerId: camera,
        snapshotImageId: container,
        switchButtonId: 'switch',
        snapButtonId: 'snap',
        stopButtonId: 'stop',
        flashOnButtonId: 'flash-on',
        flashOffButtonId: 'flash-off'
    });
    
}

function closeContainer(containerid) {
    document.getElementById(containerid).style = "display: none;";
    if (currentCameraApp && typeof currentCameraApp.stopCamera === 'function') {
        currentCameraApp.stopCamera(); // stop the camera before hiding UI
        currentCameraApp = null;       // clear reference
    }
       
}

function isBase64Image(str) {
        const base64Pattern = /^data:image\/(png|jpeg|jpg|gif|webp|bmp|svg\+xml);base64,[A-Za-z0-9+/=]+$/;
        return base64Pattern.test(str);
    }

    profile.addEventListener('load',(e)=>{
        if(profile.complete && profile.naturalWidth !== 0)
        {
            
            if(isBase64Image(profile.src))
            {
                    document.getElementById('profilehiddenvalue').value=profile.src;
            }
            else{
                document.getElementById('profilehiddenvalue').value="";
            }
            console.log(isBase64Image(profile.src));
        }
    });
    house.addEventListener('load',(e)=>{
        if(house.complete)
        {
            if((isBase64Image(house.src))===true)
            {
                document.getElementById('househiddenvalue').value=house.src;
            }
        }
    });
     pan.addEventListener('load',(e)=>{
        if(house.complete)
        {
            document.getElementById('pancardhiddenvalue').value=pan.src;
        }
    });
     aadhaar.addEventListener('load',(e)=>{
        if(aadhaar.complete)
        {
            document.getElementById('aadhaarhiddenvalue').value=aadhaar.src;
            console.log(document.getElementById('aadhaarhiddenvalue').value);
        }
    });
    
    

//
   var uploadhouseBtn = document.getElementById("houseuploadbtn");
    var uploadPanbtn=document.getElementById("pancarduploadbtn");
    var uploadaadhaarBtn=document.getElementById("aadhaarpicuploadbtn");
    var uploadProfilebtn=document.getElementById("profileuploadbtn");
    uploadProfilebtn.addEventListener('change', (e) => {
                const currFiles = e.target.files;
                if (currFiles.length > 0) {
                    const file = currFiles[0];
                    const reader = new FileReader();
                    reader.onload = () => {
                            let imagePreview = document.getElementById('profilePic');
                            imagePreview.src = reader.result; // This is the Base64 data URL — the serialization
                    };
                reader.readAsDataURL(file);
        }
    });
    uploadhouseBtn.addEventListener('change', (e) => {
                const currFiles = e.target.files;
                if (currFiles.length > 0) {
                    const file = currFiles[0];
                    const reader = new FileReader();
                    reader.onload = () => {
                            let imagePreview = document.getElementById('housePic');
                            imagePreview.src = reader.result; // This is the Base64 data URL — the serialization
                    };
                reader.readAsDataURL(file);
        }
    });




uploadPanbtn.addEventListener('change', (e) => {
    const currFiles = e.target.files;
                if (currFiles.length > 0) {
                    const file = currFiles[0];
                    const reader = new FileReader();
                    reader.onload = () => {
                            let imagePreview = document.getElementById('panPic');
                            imagePreview.src = reader.result; // This is the Base64 data URL — the serialization
                    };
                reader.readAsDataURL(file);
        }
});

uploadaadhaarBtn.addEventListener('change', (e) => {
    const currFiles = e.target.files;
                if (currFiles.length > 0) {
                    const file = currFiles[0];
                    const reader = new FileReader();
                    reader.onload = () => {
                            let imagePreview = document.getElementById('aadhaarPic');
                            imagePreview.src = reader.result; // This is the Base64 data URL — the serialization
                    };
                reader.readAsDataURL(file);
        }
});


    function clearForm() {
        document.querySelectorAll('input[type="text"], textarea,input[type="file"]').forEach(e => e.value = '');
        document.getElementById('housePic').src="../img/house.png";
        document.getElementById('aadhaarPic').src="../img/aadhaarpics.png";
        document.getElementById('panPic').src="../img/Pan-Card.png";
        document.getElementById('profilePic').src="../img/profile.png";
    }