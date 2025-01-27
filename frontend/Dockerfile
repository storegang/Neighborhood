FROM node:20-alpine
WORKDIR /usr/local/bin

EXPOSE 3000

ENV PORT=3000

ADD "node_modules" "node_modules"
ADD "package.json" "package.json"
ADD ".next" ".next"
ADD "public" "public"
ADD "next.config.js" "next.config.js"

CMD ["npm", "start"]