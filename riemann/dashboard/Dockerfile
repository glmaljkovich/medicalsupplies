FROM ruby:2.5.8

LABEL author alejandrocastrounqui@gmail.com

RUN DEBIAN_FRONTEND=noninteractive \
    apt-get update && \
    apt-get install -y \
    ca-certificates

RUN gem install --no-document riemann-dash thin 
RUN gem install --no-document fog -v 1.19.0

WORKDIR /app

COPY config.rb /app/

EXPOSE 4567

CMD [ "riemann-dash" ]