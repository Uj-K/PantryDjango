document.getElementById("startOcr").addEventListener("click", async () => {
    const video = document.getElementById("ocr-video");
    const canvas = document.getElementById("ocr-canvas");

    try {
        // 1. 카메라 접근
        const stream = await navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } });
        video.srcObject = stream;

        // 2. 영상 재생 보장
        await new Promise(resolve => {
            video.onloadedmetadata = () => {
                video.play();
                resolve();
            };
        });

        // 3. 0.5초 뒤 자동 캡처 (유저가 별도 버튼 안 눌러도 됨)
        setTimeout(() => {
            canvas.width = video.videoWidth * 2; // 해상도 증가
            canvas.height = video.videoHeight * 2; // 해상도 증가
            const context = canvas.getContext("2d");
            context.drawImage(video, 0, 0, canvas.width, canvas.height);

            // 4. 캡처된 이미지를 blob으로 변환
            canvas.toBlob(async (blob) => {
                if (!blob) {
                    alert("Image capture failed");
                    return;
                }

                const formData = new FormData();
                formData.append("imageFile", blob, "ocr-capture.jpg");

                // 5. 서버로 OCR 요청
                const response = await fetch("/FoodItems/OcrOnly", {
                    method: "POST",
                    body: formData
                });

                const result = await response.text();
                alert("OCR recognition results:\n" + result);
                if (result && !result.includes("Fail")) {
                    document.getElementById("ExpirationDate").value = result;
                } else {
                    alert("The expiration date was not recognized. Please try again.!");
                }

                // 6. 카메라 끄기
                stream.getTracks().forEach(track => track.stop());
            }, "image/jpeg");
        }, 500); // 0.5초 대기 후 캡처

    } catch (err) {
        console.error("Camera access error:", err);
        alert("Camera is not available.");
    }
});
