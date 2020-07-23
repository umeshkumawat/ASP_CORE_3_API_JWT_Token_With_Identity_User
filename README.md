# ASP_CORE_3_API_JWT_Token_With_Identity_User

इस example में हम Identity को use  करके JWT token generate करेगे। 
1. हम identity user को login नहीं करेंगे बल्कि उस user के Claims और Roles के साथ JWT token बनाएँगे।
2. JWT token को use karke हम एक endpoint को protect करेंगे।
2. User को Authorize करेंगे Roles और Policy के साथ।

# Role vs Policy 
अगर आप predifined Roles को use करते हो तो [Authorize] attribute में केवल role को specify करना होता है। और अगर हम custom role बना रहे हैं तो उसे Authorize servie में एक policy बना कर AuthorizeHandler से handle करना होता है।
