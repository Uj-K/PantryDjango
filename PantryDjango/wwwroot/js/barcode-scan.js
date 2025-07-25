// 바코드 스캔 기능을 구현하기 위한 JavaScript 코드 Xzing 라이브러리를 사용합니다.
import { BrowserMultiFormatReader } from 'https://cdn.jsdelivr.net/npm/@zxing/browser@latest/+esm';

const codeReader = new BrowserMultiFormatReader();
const videoElement = document.getElementById('video-preview');

document.getElementById('startScan').addEventListener('click', async () => {
    try {
        // ZXing 라이브러리로 바코드 스캔 시작
        await codeReader.decodeOnceFromVideoDevice(
            undefined,  // 장치를 직접 고르지 않고 constraints로 선택
            videoElement,
            (result, error) => {
                if (result) {
                    const barcode = result.getText();
                    document.getElementById("Barcode").value = barcode;

                    // 카메라 종료
                    const stream = videoElement.srcObject;
                    if (stream) {
                        stream.getTracks().forEach(track => track.stop());
                        videoElement.srcObject = null;
                    }

                    // OpenFoodFacts API 호출
                    fetch(`https://world.openfoodfacts.org/api/v0/product/${barcode}.json`)
                        .then(res => res.json())
                        .then(data => {
                            if (data.status === 1) {
                                const product = data.product;
                                document.getElementById("Name").value = product.product_name || "";
                                document.getElementById("Quantity").value = product.quantity || "";
                                document.getElementById("Categories").value = product.categories || "";
                                document.getElementById("Description").value = product.ingredients_text || "";
                            } else {
                                alert("The product could not be found.");
                            }
                        });
                }
            },
            {
                video: {
                    facingMode: { ideal: "environment" } // 후면 카메라 사용
                }
            }
        );

    } catch (err) {
        console.error("Error during barcode scanning:", err);
        alert("An error occurred during scanning.");
    }
});
