#!/bin/bash
set -e -E

# Kiểm tra API Key
if [ -z "$GEMINI_API_KEY" ]; then
    echo "Error: GEMINI_API_KEY is not set."
    exit 1
fi

MODEL_ID="gemini-2.0-flash-exp" # Cập nhật đúng ID hiện tại
INPUT_TEXT="Create a minimalist logo for a tech startup"

# Tạo request.json (Sửa lỗi trailing comma)
cat << EOF > request.json
{
    "contents": [
      {
        "role": "user",
        "parts": [
          {
            "text": "$INPUT_TEXT"
          }
        ]
      }
    ],
    "generationConfig": {
      "responseModalities": ["IMAGE", "TEXT"],
      "maxOutputTokens": 8192
    }
}
EOF

# Thực hiện Request
# Sử dụng v1beta vì Image Generation thường nằm ở bản beta
curl -X POST \
     -H "Content-Type: application/json" \
     "https://generativelanguage.googleapis.com/v1beta/models/${MODEL_ID}:generateContent?key=${GEMINI_API_KEY}" \
     -d @request.json \
     -o response.json

echo "Response saved to response.json"
