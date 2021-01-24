# CheckOut
API Gateway Test

1. Run by calling "dotnet run" in windows command prompt while in folder CheckOut\CheckOut.Com.PaymentServices
2. Has Swagger capabilty. Use http://localhost:5000/ to get swagger U.I to call API
3. App has logging functionality the logs can be seen in console and log file created in CheckOut.Com.PaymentServices\Logs

# Assumptions
The functionality is meant to work in the same session. Only Payment IDs generated in same session will work as data is not persisted

# How To Use

1. process payment API Request body example as follows

{
  "cardNumber": "1111222233334444",
  "expiry": "2021-01-24T17:04:52.844Z",
  "amount": 3,
  "currency": "GBP",
  "cvv": "123"
}

Bank has been configured to validate "cardNumber": "1111222233334444". You must use this credit card number
The balance on this card is £100. Once this has been exceeded no payments will be accepted.

example curl below:
curl -X POST "http://localhost:5000/Payments/ProcessPayment" -H "accept: */*" -H "Content-Type: application/json-patch+json" -d "{\"cardNumber\":\"1111222233334444\",\"expiry\":\"2021-01-24T17:04:52.844Z\",\"amount\":10,\"currency\":\"GBP\",\"cvv\":\"123\"}"

2. GetPayment Details

use the GUID returns in the response body from ProcessPayment API Call

example curl below:
curl -X GET "http://localhost:5000/Payments/GetPaymentDetails/{PaymentId}" -H "accept: */*"