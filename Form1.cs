using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Contentsafety_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput.Text))
            {
                // Append the user input to the chat history  
                rtbChatHistory.AppendText("You: " + txtInput.Text + "\n");

                // Clear the input TextBox  
                txtInput.Clear();

                // Simulate a response from the AI  
                rtbChatHistory.AppendText("AI: I'm an AI assistant here to help you with any questions you have about the Tesla Cybertruck. How can I assist you today?\n");
            }
        }

        private async void textBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the beep sound and newline character in the text box  
                await ProcessInputAsync(); // Call the async method  
            }
        }
        private async Task ProcessInputAsync()
        {
            string textToAnalyze = txtInput.Text;

            // Replace with your own subscription_key and endpoint for Azure Content Safety  
            string azureSubscriptionKey = "<subscription_key>";
            string azureEndpoint = "<endpoint>";

            // Replace with your own API key and endpoint for OpenAI GPT-4  
            string openAiApiKey = "<gpt4 subscription_key>";
            string openAiEndpoint = "<gpt4 endpoint>";

            // Azure Cognitive Search  
            string searchKey = "<Azure Search Key>";
            string searchEndpoint = "<Azure Search endpoint>";
            string searchIndex = "<search vector>";

            // Static category name and version  
            string categoryName = "<Azure Custom Safety Category Name>";
            int categoryVersion = <Azure Custom Category Version>;


            // Set up the API request for Azure Content Safety  
            string azureUrl = azureEndpoint + "/contentsafety/text:analyzeCustomCategory?api-version=2024-02-15-preview";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureSubscriptionKey);

            var requestBody = new
            {
                Text = textToAnalyze,
                CategoryName = categoryName,
                Version = categoryVersion
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            // Send the API request to Azure Content Safety  
            var azureResponse = await client.PostAsync(azureUrl, content);

            // Handle the API response from Azure Content Safety  
            if (azureResponse.IsSuccessStatusCode)
            {
                string result = await azureResponse.Content.ReadAsStringAsync();
                var azureResult = JsonConvert.DeserializeObject<dynamic>(result);

                bool detected = azureResult.customCategoryAnalysis.detected;

                if (detected)
                {
                    rtbChatHistory.Text = "I am sorry, I cannot comment on other brands.";
                }

                else
                {
                    // Query Azure Cognitive Search  
                    string searchUrl = $"{searchEndpoint}/indexes/{searchIndex}/docs/search?api-version=2020-06-30";
                    using HttpClient searchClient = new HttpClient();
                    searchClient.DefaultRequestHeaders.Add("api-key", searchKey);

                    var searchRequestBody = new
                    {
                        search = textToAnalyze,
                        top = 1
                    };
                    var searchContent = new StringContent(JsonConvert.SerializeObject(searchRequestBody), Encoding.UTF8, "application/json");

                    var searchResponse = await searchClient.PostAsync(searchUrl, searchContent);

                    if (searchResponse.IsSuccessStatusCode)
                    {
                        string searchResult = await searchResponse.Content.ReadAsStringAsync();
                        var searchDocuments = JsonConvert.DeserializeObject<dynamic>(searchResult);

                        // Extract relevant information from search results  
                        string retrievedDocuments = string.Join("\n", searchDocuments.value);

                        // Set up the API request for OpenAI GPT-4 with retrieved documents as context  
                        var gptRequestBody = new
                        {
                            model = "gpt-4o-bk",
                            messages = new[]
                            {
                                new { role = "system", content = "You are a helpful assistant that answers questions about Tesla & Tesla cybertrucks only." },
                                new { role = "user", content = textToAnalyze },
                                new { role = "assistant", content = retrievedDocuments }
                            }
                        };
                        var gptContent = new StringContent(JsonConvert.SerializeObject(gptRequestBody), Encoding.UTF8, "application/json");

                        using HttpClient gptClient = new HttpClient();
                        gptClient.DefaultRequestHeaders.Add("api-key", openAiApiKey);

                        var gptResponse = await gptClient.PostAsync(openAiEndpoint, gptContent);

                        if (gptResponse.IsSuccessStatusCode)
                        {
                            string gptResult = await gptResponse.Content.ReadAsStringAsync();
                            var gptResponseData = JsonConvert.DeserializeObject<dynamic>(gptResult);

                            // Print the response from GPT-4  
                            rtbChatHistory.Text = gptResponseData.choices[0].message.content.ToString();
                        }
                        else
                        {
                            rtbChatHistory.Text = "Error in GPT-4 request: " + gptResponse.StatusCode;
                        }
                    }
                    else
                    {
                        rtbChatHistory.Text = "Error in Azure Cognitive Search request: " + searchResponse.StatusCode;
                    }
                }
            }
            else
            {
                rtbChatHistory.Text = "Error in Azure Content Safety request: " + azureResponse.StatusCode;
            }
        }

        
        private void rtbChatHistory_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

    

