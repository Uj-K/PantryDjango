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
            canvas.width = video.videoWidth;
            canvas.height = video.videoHeight;
            const context = canvas.getContext("2d");
            context.drawImage(video, 0, 0, canvas.width, canvas.height);

            // 4. 캡처된 이미지를 blob으로 변환
            canvas.toBlob(async (blob) => {
                if (!blob) {
                    alert("이미지 캡처 실패");
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
                if (result && !result.includes("실패")) {
                    document.getElementById("ExpirationDate").value = result;
                } else {
                    alert("유통기한을 인식하지 못했어요. 다시 시도해주세요!");
                }

                // 6. 카메라 끄기
                stream.getTracks().forEach(track => track.stop());
            }, "image/jpeg");
        }, 500); // 0.5초 대기 후 캡처

    } catch (err) {
        console.error("카메라 접근 오류:", err);
        alert("카메라를 사용할 수 없습니다.");
    }
});
