# ASP.NET Core Hybrid X25519MLKEM768 TLS Example

1. Create a network for the server and client to communicate

```
docker network create pqc-net
```

2. Build 

```
docker build -t pqc-server .
```

3. Run

```
docker run -d --rm --name pqc-server --network pqc-net pqc-server
```

3. Verify from a PQC-enabled client

```
docker run --rm -it --network pqc-net debian:trixie sh -c "
    apt-get update -qq && apt-get install -y -qq curl openssl ca-certificates; 
    
    echo '---------------------------------------------';
    echo '1. Testing with OpenSSL s_client (Handshake Debug)';
    echo '---------------------------------------------';
    # We pipe 'Q' to quit immediately after handshake
    echo 'Q' | openssl s_client -connect pqc-server:443 -ign_eof 2>/dev/null | grep -E 'Peer signature type|Server Temp Key';

    echo '---------------------------------------------';
    echo '2. Testing with Curl (HTTP Request)';
    echo '---------------------------------------------';
    # -k (insecure) is needed because we use a self-signed cert
    curl -k -v https://pqc-server
"
```