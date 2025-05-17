captureBtn.addEventListener("click", function () {
    const context = canvas.getContext("2d");
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    context.drawImage(video, 0, 0, canvas.width, canvas.height);

    // ✅ 비동기 안에서 모든 처리
    canvas.toBlob(async function (blob) {
        if (!blob) {
            alert("이미지를 캡처하지 못했어요. 다시 시도해주세요.");
            return;
        }

        const formData = new FormData();
        formData.append("imageFile", blob, "capture.jpg");

        try {
            const response = await fetch("/Food/OcrOnly", {
                method: "POST",
                body: formData
            });

            const result = await response.text();
            if (result && !result.includes("실패")) {
                document.getElementById("ExpirationDate").value = result;
            } else {
                alert("유통기한을 인식하지 못했어요. 다시 찍어주세요!");
            }
        } catch (error) {
            console.error("OCR 요청 실패:", error);
            alert("서버에 연결할 수 없습니다.");
        }
    }, "image/jpeg");
});
