// 바코드 스캔 기능을 구현하기 위한 JavaScript 코드 Xzing 라이브러리를 사용합니다.
import { BrowserMultiFormatReader } from 'https://cdn.jsdelivr.net/npm/@zxing/browser@latest/+esm';

const codeReader = new BrowserMultiFormatReader();
const videoElement = document.getElementById('video-preview');

document.getElementById('startScan').addEventListener('click', async () => {
    try {
        // 사용자가 Scan 버튼을 클릭하면 카메라를 시작하는 곳
        const devices = await BrowserMultiFormatReader.listVideoInputDevices();
        if (devices.length === 0) {
            alert("Camera not found.");
            return;
        }
        // 카메라로 바코드를 스캔
        const result = await codeReader.decodeOnceFromVideoDevice(devices[0].deviceId, videoElement);
        const barcode = result.text;

        // 스캔된 바코드 값을 입력 필드에 설정
        document.getElementById("Barcode").value = barcode;

        // 카메라 종료
        const stream = videoElement.srcObject;
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
            videoElement.srcObject = null;
        }

        // 이후 바코드 정보를 이용해 Open Food Facts API를 호출하여 제품 정보를 가져옴
        const res = await fetch(`https://world.openfoodfacts.org/api/v0/product/${barcode}.json`);
        const data = await res.json();

        // API 응답의 status가 1(성공)이면, 상품 정보를 각 input에 자동으로 채움
        if (data.status === 1) {
            const product = data.product;
            document.getElementById("Name").value = product.product_name || "";
            document.getElementById("Quantity").value = product.quantity || "";
            document.getElementById("Categories").value = product.categories || "";
            document.getElementById("Description").value = product.ingredients_text || "";
        } else {
            alert("The product could not be found.");
        }

    } catch (err) {
        console.error(err);
        alert("An error occurred during scanning.");
    }
});
