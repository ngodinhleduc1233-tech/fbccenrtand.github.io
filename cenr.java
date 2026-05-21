// Sử dụng try-with-resources để tự động đóng file
static void saveBinaryFile(String fileName, byte[] content) {
    try (FileOutputStream out = new FileOutputStream(fileName)) {
        out.write(content);
        System.out.println("Successfully saved image: " + fileName);
    } catch (IOException e) {
        System.err.println("Critical IO Error: " + e.getMessage());
    }
}

// Trong main, xử lý tên file bằng Timestamp để tránh ghi đè
String fileName = "output_" + System.currentTimeMillis() + "." + fileExtension;
