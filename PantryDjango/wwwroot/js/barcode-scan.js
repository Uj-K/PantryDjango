import { BrowserMultiFormatReader } from 'https://cdn.jsdelivr.net/npm/@zxing/browser@latest/+esm';

const codeReader = new BrowserMultiFormatReader();
const videoElement = document.getElementById('video-preview');

document.getElementById('startScan').addEventListener('click', async () => {
    try {
        const devices = await BrowserMultiFormatReader.listVideoInputDevices();
        if (devices.length === 0) {
            alert("Camera not found.");
            return;
        }

        const result = await codeReader.decodeOnceFromVideoDevice(devices[0].deviceId, videoElement);
        const barcode = result.text;

        document.getElementById("Barcode").value = barcode;

        // 카메라 종료
        const stream = videoElement.srcObject;
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
            videoElement.srcObject = null;
        }

        const res = await fetch(`https://world.openfoodfacts.org/api/v0/product/${barcode}.json`);
        const data = await res.json();

        if (data.status === 1) {
            const product = data.product;
            document.getElementById("Name").value = product.product_name || "";
            document.getElementById("Quantity").value = product.quantity || "";
            document.getElementById("Categories").value = product.categories || "";
        } else {
            alert("The product could not be found.");
        }

    } catch (err) {
        console.error(err);
        alert("An error occurred during scanning.");
    }
});
