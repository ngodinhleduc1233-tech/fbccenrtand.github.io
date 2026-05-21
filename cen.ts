import { writeFile } from 'fs/promises'; // Sử dụng phiên bản Promise

async function saveBinaryFile(fileName: string, content: Buffer) {
  try {
    // KHÔNG dùng 'utf8' cho dữ liệu hình ảnh
    await writeFile(fileName, content); 
    console.log(`[Success] Saved: ${fileName}`);
  } catch (err) {
    console.error(`[Error] Failed to write ${fileName}:`, err);
  }
}

// Trong vòng lặp for await:
if (chunk.candidates?.[0]?.content?.parts?.[0]?.inlineData) {
  const inlineData = chunk.candidates[0].content.parts[0].inlineData;
  const fileExtension = mime.getExtension(inlineData.mimeType || 'image/png');
  const fileName = `output_${Date.now()}_${fileIndex++}.${fileExtension}`;
  
  // Chuyển đổi base64 sang Buffer một cách an toàn
  const buffer = Buffer.from(inlineData.data, 'base64');
  await saveBinaryFile(fileName, buffer); // Dùng await để đảm bảo thứ tự
}
