# StarWars EventBus

This is an OpenAPI specification defining the StarWars EventHub API, which centers on processing orders.
It features a single POST endpoint at /api/Order/Process designed to accept order requests. 
Essentially, this endpoint handles incoming orders by streaming them into an event hub system for asynchronous processing.
The design reflects an event-driven microservices architecture, where each order request acts as an independent event sent to the StarWars EventHub for efficient handling.
This approach exemplifies the "Fire & Forget" pattern, enabling seamless, asynchronous event processing at its best.

## API Endpoint
- **POST /api/Order/Process**: Accepts order requests and streams them into the event hub for processing.
- **Request Body**: Expects an `OrderRequest` object containing order details.
- **Responses**:
  - `200 OK`: Indicates that the order has been successfully received and processed.
  - `400 Bad Request`: Indicates that the request was invalid or malformed.

<img width="1514" height="675" alt="image" src="https://github.com/user-attachments/assets/c0cba97d-1b52-4b9f-9ada-eca23e322536" />
