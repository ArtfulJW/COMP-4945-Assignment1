/*
 * Program.cs will be compiled as server.exe, with additional dependencies.
 * 
 * ========================
 * 
 * Server will infinitely recurse (Keep the socket open for connection) and wait for potential users to make a connection to the ServerSocket.
 * When a connection doest finally happen, the server will create a "ServerThread(Socket)", which is a thread the server creates
 * to deal with any connections that happen.

 * =======================
 * 
 * Depending on the type of User-Agent that a connection has, the server will serve up different interactions.
 * User-Agent: CLI
 * The server expects a POST Request from this type of User-Agent, but will still check the type of request anyways.
 * 
 * User-Agent: Browser "So, NOT CLI"
 * Server will check for a GET Request for the HTML Page that the browser is asking for, and respond back with it.
 * later on, the browser will then use the UploadServlet, to make a POST Request, similiar to how the CLI does, with 
 * multi-part form request containing all the details for uploading a image.
 * 
 * =======================
 */

