# ASP.NET Core Pure Post-Quantum TLS Example

This sample demonstrates a pure post-quantum TLS 1.3 connection using:

- **ML-DSA-65** for server authentication (certificate signatures)
- **ML-KEM-768** for key exchange (key agreement)

Together, these eliminate all classical cryptography from the TLS handshake.

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

Run a PQC-enabled client (e.g., curl in a Debian Trixie container).
We explicitly pass `-groups mlkem768` / `--curves mlkem768` to force a pure ML-KEM key exchange:

```bash
docker run --rm -it --network pqc-net debian:trixie sh -c "
    apt-get update -qq && apt-get install -y -qq curl openssl ca-certificates; 
    
    echo '---------------------------------------------';
    echo '1. Testing with OpenSSL s_client (Handshake)';
    echo '---------------------------------------------';
    echo 'Q' | openssl s_client -connect tls-pure-pqc:443 -groups mlkem768 -ign_eof 2>/dev/null | grep -E 'Peer signature type|Server Temp Key';

    echo '---------------------------------------------';
    echo '2. Testing with Curl (HTTP Request)';
    echo '---------------------------------------------';
    curl -k -v --curves mlkem768 https://tls-pure-pqc
"
```

### Expected Output

- Peer signature type: `mldsa65` (pure PQC authentication)
- Server Temp Key: `mlkem768` (pure PQC key exchange)
- No classical algorithms (RSA, ECDSA, X25519, etc.) in the handshake