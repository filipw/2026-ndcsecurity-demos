# ASP.NET Core Pure ML-DSA-65 TLS Example

This sample demonstrates a pure post-quantum TLS connection using ML-DSA-65 certificates.

## 1. Setup Network

```bash
docker network create pqc-net
```

## 2. Build

```bash
docker build -t tls-pure-pqc .
```

## 3. Run Server

```bash
docker run -d --rm --name tls-pure-pqc --network pqc-net tls-pure-pqc
```

## 4. Verify

Run a PQC-enabled client (e.g., curl in a Debian Trixie container):

```bash
docker run --rm -it --network pqc-net debian:trixie sh -c "
    apt-get update -qq && apt-get install -y -qq curl openssl ca-certificates; 
    
    echo '---------------------------------------------';
    echo '1. Testing with OpenSSL s_client (Handshake Debug)';
    echo '---------------------------------------------';
    echo 'Q' | openssl s_client -connect tls-pure-pqc:443 -ign_eof 2>/dev/null | grep -E 'Peer signature type|Server Temp Key';

    echo '---------------------------------------------';
    echo '2. Testing with Curl (HTTP Request)';
    echo '---------------------------------------------';
    curl -k -v https://tls-pure-pqc
"
```