import { Anchor, Box, Group, List, Text } from '@mantine/core';

export default function Index() {
  return (
    <Group position="center">
      <Box p="xl">
        <Text
          component="span"
          inherit
          variant="gradient"
          gradient={{ from: 'pink', to: 'yellow' }}
          sx={{ fontSize: '3em' }}
        >
          Welcome to Remix
        </Text>
        <List>
          <List.Item>
            <Anchor
              target="_blank"
              href="https://remix.run/tutorials/blog"
              rel="noreferrer"
            >
              15m Quickstart Blog Tutorial
            </Anchor>
          </List.Item>
          <List.Item>
            <Anchor
              target="_blank"
              href="https://remix.run/tutorials/jokes"
              rel="noreferrer"
            >
              Deep Dive Jokes App Tutorial
            </Anchor>
          </List.Item>
          <List.Item>
            <Anchor
              target="_blank"
              href="https://remix.run/docs"
              rel="noreferrer"
            >
              Remix Docs
            </Anchor>
          </List.Item>
        </List>
      </Box>
    </Group>
  );
}
