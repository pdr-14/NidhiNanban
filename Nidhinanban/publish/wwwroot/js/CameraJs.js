function setupCameraApp({
    videoContainerId,
    snapshotImageId,
    switchButtonId,
    snapButtonId,
    stopButtonId,
    flashOnButtonId,
    flashOffButtonId
}) {
    let currentStream = null;
    let currentTrack = null;
    let availableCameras = [];
    let currentCameraIndex = 0;
    let frontcam=false;

    const video = document.createElement('video');
    video.autoplay = true;
    video.playsInline = true;
    video.width = 320;
    video.height = 240;
    video.classList.add('d-block', 'mx-auto');

    const videoContainer = document.getElementById(videoContainerId);
    const snapshotImage = document.getElementById(snapshotImageId);
    const switchButton = document.getElementById(switchButtonId);
    const snapButton = document.getElementById(snapButtonId);
    const stopButton = document.getElementById(stopButtonId);
    const flashOnButton = document.getElementById(flashOnButtonId);
    const flashOffButton = document.getElementById(flashOffButtonId);
    
    videoContainer.appendChild(video);

    async function getAvailableCameras() {
        const devices = await navigator.mediaDevices.enumerateDevices();
        availableCameras = devices.filter(device => device.kind === 'videoinput');
    }
//deviceId ? { deviceId: { exact: deviceId } } 
    async function getCameraStream(deviceId = null) {
        console.log(frontcam);
        const constraints = {
            video: { facingMode: frontcam?"user": "environment"  },
            audio: false
        };
        try {
            const stream = await navigator.mediaDevices.getUserMedia(constraints);
            if (currentStream) {
                currentStream.getTracks().forEach(track => track.stop());
            }
            currentStream = stream;
            currentTrack = stream.getVideoTracks()[0];
            video.srcObject = stream;
        } catch (err) {
            console.error('Camera error:', err);
        }
    }

    async function switchCamera() {
        if (availableCameras.length > 1) {
            currentCameraIndex = (currentCameraIndex + 1) % availableCameras.length;
            getCameraStream(availableCameras[currentCameraIndex].deviceId);
            if(frontcam)
            {
                frontcam=false;
            }
            else{
                frontcam=true;
            }
        } else {
            console.warn("Only one camera detected.");
        }
    }

    async function toggleFlashlight(turnOn) {
        try {
            if (!currentTrack) {
                console.warn("No active video track for flashlight control.");
                return;
            }

            const capabilities = currentTrack.getCapabilities();
            if (capabilities.torch) {
                await currentTrack.applyConstraints({
                    advanced: [{ torch: turnOn }]
                });
            } else {
                console.warn("Torch not supported on this device.");
            }
        } catch (err) {
            console.error("Flashlight error:", err);
        }
    }

    // Initialize camera setup
    (async () => {
        await getAvailableCameras();
        getCameraStream(availableCameras[currentCameraIndex]?.deviceId);
    })();

    switchButton?.addEventListener('click', switchCamera);

    snapButton?.addEventListener('click', () => {
        const canvas = document.createElement('canvas');
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        const ctx = canvas.getContext('2d');
        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
        snapshotImage.src = canvas.toDataURL('image/png');
    });

    function stopCamera() {
        if (currentStream) {
            currentStream.getTracks().forEach(track => track.stop());
            currentStream = null;
            currentTrack = null;
            video.srcObject = null;
            video.remove();
        }
    }

    stopButton?.addEventListener('click', () => {
        if (currentStream) {
            currentStream.getTracks().forEach(track => track.stop());
            currentStream = null;
            currentTrack = null;
            video.srcObject = null;
            video.remove();
            
        }
    });

    flashOnButton?.addEventListener('click', () => {
        toggleFlashlight(true);
    });

    flashOffButton?.addEventListener('click', () => {
        toggleFlashlight(false);
    });
    
    return {
        stopCamera // <- this makes it accessible from outside
    };
}