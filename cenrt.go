package main

import (
	"context"
	"flag"
	"fmt"
	"log"
	"os" // Đã thêm thư viện os

	"google.golang.org/genai"
)

func run(ctx context.Context) {
	apiKey := os.Getenv("GEMINI_API_KEY")
	if apiKey == "" {
		log.Fatal("GEMINI_API_KEY is not set")
	}

	client, err := genai.NewClient(ctx, &genai.ClientConfig{
		APIKey: apiKey,
	})
	if err != nil {
		log.Fatal(err)
	}

	model := flag.String("model", "gemini-2.0-flash", "model name")
	flag.Parse()

	config := &genai.GenerateContentConfig{
		MaxOutputTokens:    8192,
		ResponseModalities: []string{"IMAGE", "TEXT"}, // Đã sửa cú pháp
	}

	contents := []*genai.Content{
		{
			Role: "user",
			Parts: []*genai.Part{
				{Text: "Create a blueprint for a mechatronic arm"},
			},
		},
	}

	result, err := client.Models.GenerateContent(ctx, *model, contents, config)
	if err != nil {
		log.Fatal(err)
	}

	// Xử lý cả Text và Image
	for _, candidate := range result.Candidates {
		for _, part := range candidate.Content.Parts {
			if part.Text != "" {
				fmt.Printf("AI Response: %s\n", part.Text)
			}
			if part.InlineData != nil {
				fmt.Printf("Received image data: %d bytes\n", len(part.InlineData.Data))
				// Tại đây bạn có thể dùng os.WriteFile để lưu ảnh
			}
		}
	}
}

func main() {
	run(context.Background())
}
