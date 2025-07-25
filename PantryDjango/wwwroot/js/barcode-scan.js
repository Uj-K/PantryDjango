// 바코드 스캔 기능을 구현하기 위한 JavaScript 코드 Xzing 라이브러리를 사용합니다.
import { BrowserMultiFormatReader } from 'https://cdn.jsdelivr.net/npm/@zxing/browser@latest/+esm';

const codeReader = new BrowserMultiFormatReader();
const videoElement = document.getElementById('video-preview');

document.getElementById('startScan').addEventListener('click', async () => {
    try {
        // 후면 카메라 우선 요청
        const result = await codeReader.decodeOnceFromVideoDevice(
            undefined,
            videoElement,
            undefined,  // callback 없음
            {
                video: {
                    facingMode: { ideal: "environment" }
                }
            }
        );

        // 스캔 결과 처리
        const barcode = result.getText();
        document.getElementById("Barcode").value = barcode;

        // 카메라 종료
        const stream = videoElement.srcObject;
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
            videoElement.srcObject = null;
        }

        // Open Food Facts API 호출
        const res = await fetch(`https://world.openfoodfacts.org/api/v0/product/${barcode}.json`);
        const data = await res.json();

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
