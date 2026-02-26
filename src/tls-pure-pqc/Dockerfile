# use Debian 13 (contains OpenSSL 3.5+)
FROM debian:trixie

RUN apt-get update && apt-get install -y \
    wget \
    openssl \
    ca-certificates \
    libicu-dev \
    && rm -rf /var/lib/apt/lists/*

ENV DOTNET_INSTALL_DIR=/usr/share/dotnet
ENV PATH=$PATH:$DOTNET_INSTALL_DIR
RUN wget https://dot.net/v1/dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && ./dotnet-install.sh --channel 10.0 --install-dir $DOTNET_INSTALL_DIR

WORKDIR /app

# generate pure ML-DSA-65 certificate (authentication side of pure PQC)
# key exchange (ML-KEM-768) is negotiated at the TLS protocol level
RUN openssl req -x509 -new -newkey mldsa65 -keyout server.key -out server.crt -nodes -days 365 \
    -subj "/CN=pqc-server" \
    -addext "subjectAltName=DNS:pqc-server,DNS:localhost,IP:127.0.0.1"

COPY Program.cs .
COPY TlsPurePqc.csproj .

RUN dotnet publish -c Release -o /app/publish

WORKDIR /app/publish
RUN cp /app/server.crt . && cp /app/server.key .

# configure OpenSSL to only offer pure ML-KEM-768 for key exchange (no hybrid, no classical)
RUN sed -i '/^\[openssl_init\]/a ssl_conf = ssl_configuration' /etc/ssl/openssl.cnf && \
    printf '\n[ssl_configuration]\nsystem_default = tls_system_default\n\n[tls_system_default]\nGroups = mlkem768\n' >> /etc/ssl/openssl.cnf

ENV ASPNETCORE_URLS=https://+:443
EXPOSE 443

ENTRYPOINT ["dotnet", "TlsPurePqc.dll"]