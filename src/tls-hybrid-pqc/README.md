# ASP.NET Core Hybrid X25519MLKEM768 TLS Example

This sample demonstrates a hybrid post-quantum TLS key exchange using standard ECDSA certificates.

## 1. Build

```bash
docker build -t tls-hybrid-pqc .
```

## 2. Run

```bash
docker run -d -p 8081:443 --name tls-hybrid-pqc tls-hybrid-pqc
```

## 3. Verify

Go to https://localhost:8081 in a browser that supports hybrid post-quantum TLS cipher suites (e.g., Chrome).

Check DevTools (F12) -> `Security` Tab. You should see "The connection to this site is encrypted and authenticated using TLS 1.3, X25519MLKEM768, and AES_256_GCM."