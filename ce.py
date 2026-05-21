def generate():
    # ... (giữ nguyên phần setup ban đầu)
    
    image_data = bytearray() # Bộ nhớ đệm để gom ảnh
    mime_type = "image/png"

    try:
        for chunk in client.models.generate_content_stream(model=model, contents=contents, config=generate_content_config):
            if not chunk.parts: continue
            
            part = chunk.parts[0]
            if part.inline_data:
                image_data.extend(part.inline_data.data)
                mime_type = part.inline_data.mime_type
            elif part.text:
                print(f"AI: {part.text}", end="")

        if image_data:
            ext = mimetypes.guess_extension(mime_type) or ".png"
            with open(f"output_image{ext}", "wb") as f:
                f.write(image_data)
            print(f"\n[Thành công] Đã lưu ảnh vào output_image{ext}")
            
    except Exception as e:
        print(f"\n[Lỗi hệ thống]: {e}")
