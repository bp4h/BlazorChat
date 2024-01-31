window.sendVideoFrame = async (data) => {
    const videoElement = document.getElementById('videoElement');
    const blob = new Blob([data], { type: 'video/webm' });
    const url = URL.createObjectURL(blob);
    videoElement.src = url;
}
