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

# generate a Standard ECDSA Certificate
WORKDIR /app

# use prime256v1 (standard ECDSA) instead of "pure PQC" such as mldsa65
RUN openssl req -x509 -new -newkey ec -pkeyopt ec_paramgen_curve:prime256v1 -keyout server.key -out server.crt -nodes -days 365 -subj "/CN=localhost"

COPY Program.cs .
COPY PqcDemo.csproj .

RUN dotnet publish -c Release -o /app/publish

WORKDIR /app/publish
RUN cp /app/server.crt . && cp /app/server.key .

ENV ASPNETCORE_URLS=https://+:443
EXPOSE 443

ENTRYPOINT ["dotnet", "PqcDemo.dll"]