# ASP.NET Core Hybrid X25519MLKEM768 TLS Example

1. Build

```
docker build -t pqc-hybrid .
```

2. Run

```
docker run -d -p 8081:443 --name pqc-hybrid-app pqc-hybrid
```

3. Verify in a browser that supports hybrid post-quantum TLS cipher suites.

Go to https://localhost:8081. The page should load successfully (ignore the certificate warning since it's self-signed).

Check DevTools (F12) -> `Privacy and Security` Tab.

You should see "The connection to this site is encrypted and authenticated using TLS 1.3, X25519MLKEM768, and AES_256_GCM." indicating the use of the hybrid post-quantum cipher suite.